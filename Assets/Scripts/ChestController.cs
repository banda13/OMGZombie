using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour {

    public GameObject player;
    public ParticleSystem particles;
    public GameObject item;
    public GameObject weapon;

    public GameObject canvas;

    private CamaraController controller;
    private Animator anim;

    private float raiseDuration = 3;
    private bool opening = false;
    private bool particlesPlayed = false;

    public bool missionStarted = false;
    public bool missionCompleted = false;
    public bool missionProgress = false;
    
    private float textAppearingTime = 2.0f;

    private void Start()
    {
        controller = player.GetComponent<CamaraController>();
        StartCoroutine(spawnText(0));
    }
    
    void Update () {
        
        //for debugging
        if (Input.GetKeyDown("space") && (Vector3.Distance(transform.position, player.transform.position) < 5))
        {
            pointerClick();
        }

        if (opening)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 5)
            {
                Animator animator = this.GetComponent<Animator>();
                animator.SetTrigger("Open");
                controller.Wait = true;
                

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                {
                    if (!particlesPlayed)
                    {
                        particles.Play();
                    }
                    particlesPlayed = true;
                    raiseDuration -= Time.deltaTime;
                    item.transform.position += new Vector3(0, 0.05f, 0);

                    if (raiseDuration < 0)
                    {
                        opening = false;
                        controller.Wait = true;
                        if (player.GetComponent<PlayerController>().weapon == null)
                        {
                            GameObject wp = Instantiate(weapon, new Vector3(0, 0, 0), player.transform.rotation) as GameObject;
                            wp.transform.parent = player.transform;
                            wp.transform.localPosition = new Vector3(0.5f, -0.6f, 0.2f);
                            //wp.transform.position = player.transform.GetChild(0).transform.position + new Vector3(0.2f, -2f, -0.2f);
                            wp.transform.rotation = player.transform.GetChild(0).transform.rotation;
                            player.GetComponent<PlayerController>().weapon = wp;
                        }
                        player.transform.GetChild(5).gameObject.SetActive(true);
                        player.GetComponent<CamaraController>().activateZombies();
                        player.GetComponent<CamaraController>().stopSnipeMission();
                        player.GetComponent<CamaraController>().speed = 0.4f;
                    }
                }
            }
        }
    }

    public void pointerClick()
    {
        if (!missionStarted && !missionCompleted)
        {
            missionStarted = true;
            player.GetComponent<CamaraController>().Wait = true;
            StartCoroutine(spawnText(1));
        }
        else if (missionStarted && missionCompleted)
        {
            missionProgress = false;
            opening = !opening;
        }
    }

    IEnumerator testCallForMissionOne()
    {
        yield return new WaitForSeconds(2);
        startMission();
    }

    private IEnumerator spawnText(float alpha)
    {
        Text text = canvas.transform.GetChild(0).GetComponent<Text>();
        Text buttonText = canvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();
       
        text.CrossFadeAlpha(alpha, textAppearingTime, true);
        buttonText.CrossFadeAlpha(alpha, textAppearingTime, true);
        yield return new WaitForSeconds(textAppearingTime);
        if (alpha > 0.5f)
        {
            canvas.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (alpha <= 0.5f)
        {
            canvas.transform.GetChild(1).gameObject.SetActive(false);
        }
    } 

    public void startMission()
    {
        missionProgress = true;
        Debug.Log("Mission started");
        canvas.SetActive(false);
        player.GetComponent<CamaraController>().startSnipeMission();
        
    }

    public bool canStartWalking(float speed)
    {
        if(missionCompleted &&  missionStarted && player.GetComponent<PlayerController>().weapon != null)
        {
            StartCoroutine(player.GetComponent<CamaraController>().delayedGo(speed));
            return true;
        }
        return false;
    }
}

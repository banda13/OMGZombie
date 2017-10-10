using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        controller = player.GetComponent<CamaraController>();

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
                        controller.Wait = false;
                        GameObject wp = Instantiate(weapon, new Vector3(0, 0, 0), player.transform.rotation) as GameObject;
                        wp.transform.parent = player.transform;
                        wp.transform.localPosition = new Vector3(0.5f, -0.6f, 0.2f);
                        //wp.transform.position = player.transform.GetChild(0).transform.position + new Vector3(0.2f, -2f, -0.2f);
                        wp.transform.rotation = player.transform.GetChild(0).transform.rotation;
                        player.GetComponent<PlayerController>().weapon = wp;
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
            canvas.transform.rotation = Quaternion.LookRotation(-(player.transform.position - transform.position));
            canvas.SetActive(true);
        }
        else if(missionStarted && !missionCompleted)
        {
            startMission();
        }
        else if (missionStarted && missionCompleted)
        {
            opening = !opening;
        }
    }

    IEnumerator testCallForMissionOne()
    {
        yield return new WaitForSeconds(2);
        startMission();
    }

    public void startMission()
    {
        Debug.Log("Mission started");
        canvas.SetActive(false);
        player.GetComponent<CamaraController>().startSnipeMission();
        
    }
}

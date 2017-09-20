using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour {

    public GameObject player;
    public ParticleSystem particles;
    public GameObject item;

    private CamaraController controller;
    private Animator anim;

    private float raiseDuration = 3;
    private bool opening = false;
    private bool particlesPlayed = false;

    private void Start()
    {
        controller = player.GetComponent<CamaraController>();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("space"))
        {
            opening = !opening;
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
                    item.transform.position += new Vector3(0, 0.01f, 0);

                    Debug.Log(raiseDuration);

                    if (raiseDuration < 0)
                    {
                        opening = false;
                        //controller.Wait = false;
                    }
                }

            }
        }
        else
        {
            //change when the player pick up the weapon
            if (controller.Wait)
            {
                controller.Wait = false;
            }
        }


    }
}

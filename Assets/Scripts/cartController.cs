using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cartController : PathFollower {

    public GameObject player;
    private bool empty = true;

    private bool right, left = false;
    
	
	void Start () {
        Wait = true;
        init();
	}
	
	void Update () {

        if (Input.GetKeyDown("space") && Vector3.Distance(transform.position, player.transform.position) < 4)
        {
            if (empty)
            {
                empty = false;
                player.transform.position = this.transform.position + new Vector3(0, 2, 0);
                player.transform.parent = this.transform;
                Wait = false;
                

            }
            else
            {
                empty = true;
                Wait = true;
                player.transform.parent = null;
                player.transform.position = this.transform.position + new Vector3(1, 1, 0);
            }
        }

        rotate();
        move();

        megdont();
        
	}

    private  void megdont()
    {
        if (Input.GetKeyDown("right"))
        {
            transform.Rotate(0.01f, 0, 0);
        }
        if (Input.GetKeyDown("left"))
        {
            transform.Rotate(-0.01f, 0, 0);
        }
    }

    public override void Go()
    {
        throw new NotImplementedException();
    }
}

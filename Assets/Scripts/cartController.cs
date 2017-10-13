using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cartController : PathFollower {

    public GameObject player;
    private bool empty = true;
    private bool moving = false;

    private bool right, left = false;
    private float neededRotation = 0;
	
	void Start () {
        Wait = true;
        init();
	}
	
	void Update () {

        if (Input.GetKeyDown("space") && Vector3.Distance(transform.position, player.transform.position) < 4)
        {
            if (empty)
            {
                getInCar();
            }
            else
            {
                getOutOfCar();
            }
        }

        rotate();
        move();

	}

    public void addExtraRotation(float rotation)
    {
        neededRotation = rotation;
        transform.rotation = transform.rotation *  Quaternion.Euler(rotation, 0, 0);
    }

    public void getInCar()
    {
        empty = false;
        player.transform.position = this.transform.position + new Vector3(0, 2, 0);
        player.transform.parent = this.transform;
        Wait = false;
    }

    public void getOutOfCar()
    {
        empty = true;
        player.transform.parent = null;
        player.transform.position = this.transform.position + new Vector3(1, 1, 0);
        Wait = true;
    }

    public override void Go()
    {
        throw new NotImplementedException();
    }
}

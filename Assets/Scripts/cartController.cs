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

    public GameObject shapeObject;
    private int prevWaypoint;
	
	void Start () {
        Wait = true;
        init();
        prevWaypoint = -1;
       
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

        if(!empty && prevWaypoint < currentWaypoint)
        {
            prevWaypoint++;
            //StartCoroutine(getNewShape());
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
        StartCoroutine(getNewShape());
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

    public IEnumerator getNewShape()
    {
        if(shapeObject.GetComponent<RailDrawer>() != null)
        {
            shapeObject.GetComponent<RailDrawer>().cart = this;
            GameObject playerCam = player.transform.GetChild(0).gameObject;
            shapeObject.GetComponent<RailDrawer>().playerCamera = playerCam;
            yield return new WaitForSeconds(2);
            GameObject shape = Instantiate(shapeObject, playerCam.transform.position + playerCam.transform.forward * 2f, Quaternion.identity);
            //return shape;
        }
        //return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cartController : PathFollower {

    public GameObject player;
    private bool empty = true;
    private bool moving = false;

    private bool right, left = false;
    private float neededRotation = 0;

    public List<GameObject> shapes;
    private int prevWaypoint;

    public float rotationLimit = 30;
    public float rotationSpeed = 10;
    public bool dying = false;
    private bool isPaused = false;

    private float maxLeftRotation = 330;
    private float maxRightRotation = 30;
    private float normalRotation = 0;

    public delegate void fadingAction();

	void Start () {
        Wait = true;
        init();
        prevWaypoint = -1;
       
    }
	
	void Update () {

        if (!dying)
        {
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

            if (!empty && prevWaypoint < currentWaypoint)
            {
                prevWaypoint++;
            }

            rotate();
            move();
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 45), Time.deltaTime * rotationSpeed *2 );
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -1, transform.position.z), Time.deltaTime * rotationSpeed/2);
        }

	}

    public void rotateLeft()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(maxLeftRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * rotationSpeed);

    }
    public void rotateRight()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(maxRightRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * rotationSpeed);
        
    }
    public void stabilaze()
    { 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(normalRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * rotationSpeed);
    }
    
    public void getInCar()
    {
        if (empty && !isPaused)
        {
            empty = false;
            player.transform.position = this.transform.position + new Vector3(0, 2, 0);
            player.transform.parent = this.transform;
            Wait = false;
            getNewShape();
        }
    }

    public void getOutOfCar()
    {
        if (!isPaused)
        {
            empty = true;
            player.transform.parent = null;
            player.transform.position = this.transform.position + new Vector3(1, 1, 0);
            Wait = true;
        }
    }

    public override void Go()
    {
        throw new NotImplementedException();
    }

    public void getNewShape()
    {
        StartCoroutine(spawnNewShape());
    }



    private IEnumerator spawnNewShape()
    {
        yield return new WaitForSeconds(3);
        GameObject shapeObject = null;
        if(shapes != null && shapes.Count != 0)
        {
            shapeObject = shapes[UnityEngine.Random.Range(0, shapes.Count)];
        }
        if (shapeObject.GetComponent<RailDrawer>() != null)
        {
            shapeObject.GetComponent<RailDrawer>().cart = this;
            GameObject playerCam = player.transform.GetChild(0).gameObject;
            shapeObject.GetComponent<RailDrawer>().playerCamera = playerCam;
            yield return new WaitForSeconds(2);
            GameObject shape = Instantiate(shapeObject, playerCam.transform.position + (Quaternion.Euler(0, -90, 0) * transform.forward ) * shapeObject.GetComponent<RailDrawer>().distanceFromCamera, Quaternion.identity);
            //return shape;(
        }
    }

    public IEnumerator failed(GameObject rail)
    {
        dying = true;
        yield return new WaitForSeconds(2);
        Destroy(rail);
        StartCoroutine(player.GetComponent<MineCamaraController>().fadingWithCartActions(failedActions));
        AdaptedEventHandler.playerDead(getCurrentWaypointPosition(), getCurrentWaypointName());
    }

    public void failedActions()
    {
        SceneManager.LoadScene("mine", LoadSceneMode.Single);
    }

    void OnPauseGame()
    {
        isPaused = true;
    }

    void OnResumeGame()
    {
        isPaused = false;
    }
}

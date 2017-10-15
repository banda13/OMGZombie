using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMission : MonoBehaviour {

    private GameObject camera;
    public bool missionStarted = false;
    public cartController cart;
    private GameObject activeShape;

	void Start () {
        camera = transform.GetChild(0).gameObject;
	}
	
	void Update () {

        if (missionStarted)
        {
            if(activeShape != null)
            {
                activeShape.transform.position = camera.transform.position + camera.transform.forward * 2f;
                activeShape.transform.LookAt(camera.transform.position);
            }
        }
	}
    
}

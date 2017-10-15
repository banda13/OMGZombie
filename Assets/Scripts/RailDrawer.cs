using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RailDrawer : MonoBehaviour {

    public GameObject playerCamera;
    private List<Collider> checkPoints;
    private int completedCheckpoints = 0;
    private int maxFail = 4;
    private float centerX;
    private bool drawing = false;

    public enum PointerPos { left, center, right};
    public PointerPos pointerPos;
    private Vector3 position;
    private Vector3 previousPosition;

    public GameObject particleObj;
    private GameObject particle;
    public cartController cart;
    private float calculatedSpeed;

    private bool breaking = true;
    

	void Start () {
        centerX = transform.position.x;
        pointerPos = PointerPos.center;
        

        checkPoints = new List<Collider>();
        foreach(Collider c in GetComponents<SphereCollider>())
        {
            c.enabled = false;
            checkPoints.Add(c);
        }
        checkPoints[0].enabled = true;
        cart.speed = 0;
        
	}
    
	void Update () {

        if(breaking && cart.speed != 0)
        {
            cart.speed -= 0.01f;
        }

        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), Time.deltaTime * 0.1f);
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * 2f;
        transform.LookAt(playerCamera.transform.position);
        if(particle != null)
        {
            particle.transform.LookAt(playerCamera.transform.position);
        }

        if (drawing)
        {
            cart.speed = calculateDrawningSpeed();
        }
	}

    private float calculateDrawningSpeed()
    {
        
        if (previousPosition.y != 0)
        {
            float pos = Math.Abs(position.y) * 200;
            float prevPos = Math.Abs(previousPosition.y) * 200;
            if (previousPosition != null && position != null)
            {
                //Debug.Log(pos + " - " + prevPos + "  a sebesség");
                if (pos - prevPos > 0)
                {

                    return (pos - prevPos) * 2;
                }
            }
            return 0;
        }
        else
        {
            return 0;
        }
    }
    
    public void startDrawing(BaseEventData e)
    {
        drawing = true;
        breaking = false;
        PointerEventData pointerData = e as PointerEventData;
        position = pointerData.pointerCurrentRaycast.worldPosition;
        previousPosition = position;
                
        if(particle == null)
        {
            particle = Instantiate(particleObj, position, Quaternion.Euler(0, 0, 0));
            particle.transform.SetParent(transform);
        }
       particle.transform.position = position;
    }

    public void pointerDrawing(BaseEventData e)
    {
        previousPosition = position;
        PointerEventData pointerData = e as PointerEventData;
        position = pointerData.pointerCurrentRaycast.worldPosition;

        if (particle == null)
        {
            particle = Instantiate(particleObj, position, Quaternion.Euler(0, 0, 0));
            particle.transform.SetParent(transform);
        }
        particle.transform.position = position;
        if (position != null)
        {     
            if (Math.Abs(position.x) > Math.Abs(transform.position.x))
            {
                pointerPos = PointerPos.left;
            }
            else if (Math.Abs(position.x) < Math.Abs(transform.position.x))
            {
                pointerPos = PointerPos.right;
            }
            else
            {
                pointerPos = PointerPos.center;
            }
        }
    }

    public void pointerOut()
    {
        if (drawing)
        {
            maxFail--;
            Debug.Log("Oops, u missed ->" + pointerPos);
            if (particle != null)
                DestroyImmediate(particle, true);
            if (pointerPos == PointerPos.left)
            {
                //cart.addExtraRotation(-30);
            }
            else if (pointerPos == PointerPos.right)
            {
                //cart.addExtraRotation(30);
            }
            if(maxFail <= 0)
            {
                cart.Wait = true;
                //we dead
            }
        }
    }
    public void stopDrawing()
    {
        drawing = false;
        if (particle != null)
            DestroyImmediate(particle, true);
    }

   

    private void missionCompleted()
    {
        StartCoroutine(cart.getNewShape());
        Debug.Log("Getting new shape");
        Destroy(this.gameObject);
        Destroy(particle);
    }

    public void checkPointCompleted(Collider other)
    {
        completedCheckpoints++;
        Debug.Log(completedCheckpoints + " chekpoint completed");
        other.enabled = false;
        if (completedCheckpoints == checkPoints.Count)
        {
            missionCompleted();
        }
        else
        {
            checkPoints[completedCheckpoints].enabled = true;
        }
    }
    
}

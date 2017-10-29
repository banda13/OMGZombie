using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RailDrawer : MonoBehaviour {

    public GameObject playerCamera;
    public cartController cart;
    public GameObject particleObj;
    private GameObject particle;

    private List<Collider> checkPoints;
    private int completedCheckpoints = 0;
    public int maxFail = 4;
    
    public bool isPointerOnObject = false;
    public bool isPointerDown = false;
    public bool isDragging = false;
    public bool isLeaningLeft = false;
    public bool isLeaningRight = false;

    private Vector3 draggingPosition;
    private Vector3 previousDraggingPosition;

    public float speedMultiplier = 200;
    public float breakingSpeed = 0.01f;
    public float maxCarSpeed = 3;

    public float distanceFromCamera = 2;


    public void pointerDown(BaseEventData e)
    {
        isPointerDown = true;
        PointerEventData pointerData = e as PointerEventData;
        draggingPosition = pointerData.pointerCurrentRaycast.worldPosition;
        previousDraggingPosition = draggingPosition;
    }

    public void pointerUp(BaseEventData e)
    {
        isPointerDown = false;
        PointerEventData pointerData = e as PointerEventData;
        draggingPosition = pointerData.pointerCurrentRaycast.worldPosition;
    }

    public void pointerEnter(BaseEventData e)
    {
        isPointerOnObject = true;


    }

    public void pointerExit(BaseEventData e)
    {
        isPointerOnObject = false;
        if(isPointerDown && isDragging)
        {
            checkPointMissed();
        }
    }
    public void startDragging(BaseEventData e)
    {
        previousDraggingPosition = draggingPosition;
        isDragging = true;
        PointerEventData pointerData = e as PointerEventData;
        draggingPosition = pointerData.pointerCurrentRaycast.worldPosition;
    }

    public void dragging(BaseEventData e)
    {
        if (isPointerOnObject)
        {
            previousDraggingPosition = draggingPosition;
            PointerEventData pointerData = e as PointerEventData;
            draggingPosition = pointerData.pointerCurrentRaycast.worldPosition;
        }
        
    }
    public void stopDragging(BaseEventData e)
    {
        isDragging = false;
        PointerEventData pointerData = e as PointerEventData;
        draggingPosition = pointerData.pointerCurrentRaycast.worldPosition;
    }

    private void createOrUpdateParticle()
    {
        if (particle == null)
        {
            particle = Instantiate(particleObj, draggingPosition, Quaternion.Euler(0, 0, 0));
            //particle.transform.SetParent(transform);
        }
        particle.transform.localPosition = draggingPosition;
    }
    

    private void destroyPartice()
    {
        if (particle != null)
            DestroyImmediate(particle, true);
    }

	void Start () {
        checkPoints = new List<Collider>();
        foreach(Collider c in GetComponents<SphereCollider>())
        {
            c.enabled = false;
            checkPoints.Add(c);
        }
        checkPoints[0].enabled = true;
	}
    
	void Update () {

        //set Shape
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * distanceFromCamera;
        transform.LookAt(playerCamera.transform.position);

        //set particle
        if (particle != null)
        {
            particle.transform.LookAt(playerCamera.transform.position);
        }
        if(isPointerOnObject && isPointerDown)
        {
            createOrUpdateParticle();
        }
        else
        {
            destroyPartice();
        }

        //set cart rotation
        if (isPointerDown && isDragging && !isPointerOnObject)
        {
                if (draggingPosition.x < previousDraggingPosition.x)
                {
                    isLeaningLeft = true;
                    cart.rotateLeft();
                }
                else
                {
                    isLeaningRight = true;
                    cart.rotateRight();
                }
        }
        else
        {
            isLeaningRight = isLeaningLeft = false;
            cart.stabilaze();
        }

        //set cart speed
        if(isPointerOnObject && isDragging && isPointerOnObject)
        {
            setCarSpeed();
        }
        else
        {
            if (cart.speed > 0)
            {
                cart.speed -= breakingSpeed;
            }
            else
            {
                cart.speed = 0;
            }
        }
	}

    private void setCarSpeed()
    {
        if(previousDraggingPosition != null && draggingPosition!= null)
        {
            float pos = Math.Abs(draggingPosition.y) * 200;
            float prevPos = Math.Abs(previousDraggingPosition.y) * 200;
            if (pos - prevPos > 0)
            {
                float newSpeed = (pos - prevPos) * 2;
                cart.speed = newSpeed < maxCarSpeed ? newSpeed : maxCarSpeed;
                return;
            }
        }
        cart.speed = 0;
    }

    private void missionCompleted()
    {
        cart.getNewShape();
        cart.speed = 2;
        Destroy(this.gameObject);
        Destroy(particle);
    }

    private void checkPointMissed()
    {
        maxFail--;
        destroyPartice();
        Debug.Log("Hiba");
        if(maxFail <= 0)
        {
            StartCoroutine(cart.failed(this.gameObject));
        }
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

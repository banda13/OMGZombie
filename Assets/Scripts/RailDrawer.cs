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
    public bool drawing = false;

    public enum PointerPos { left, center, right};
    public PointerPos pointerPos;
    private Vector3 position;
    private Vector3 previousPosition;

    public GameObject particleObj;
    private GameObject particle;
    public cartController cart;
    private float calculatedSpeed;

    private bool breaking = true;
    

    public bool isPointerOnObject = false;
    public bool isPointerDown = false;
    public bool isDragging = false;
    public bool isLeaningLeft = false;
    public bool isLeaningRight = false;

    private Vector3 draggingPosition;
    private Vector3 previousDraggingPosition;

    private float speedMultiplier = 200;
    public float breakingSpeed = 0.01f;


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
            particle.transform.SetParent(transform);
        }
        particle.transform.position = draggingPosition;
    }
    

    private void destroyPartice()
    {
        if (particle != null)
            DestroyImmediate(particle, true);
    }

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

        //set Shape
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * 4f;
        transform.LookAt(playerCamera.transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), Time.deltaTime * 0.1f);

        //set particle
        if (particle != null)
        {
            particle.transform.LookAt(playerCamera.transform.position);
        }
        if(isPointerOnObject && isPointerDown)
        {
            //createOrUpdateParticle();
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
        if(previousDraggingPosition != null && position!= null)
        {
            float pos = Math.Abs(draggingPosition.y) * 200;
            float prevPos = Math.Abs(previousDraggingPosition.y) * 200;
            if (pos - prevPos > 0)
            {
                float newSpeed = (pos - prevPos) * 2;
                cart.speed = newSpeed < 2 ? newSpeed : 2;
                return;
            }
        }
        cart.speed = 0;
    }

    private void missionCompleted()
    {
        cart.getNewShape();
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

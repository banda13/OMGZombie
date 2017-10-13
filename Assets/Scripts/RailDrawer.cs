using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RailDrawer : MonoBehaviour {

    private List<Collider> checkPoints;
    private int completedCheckpoints = 0;
    private int maxFail = 4;
    private float centerX;
    private bool drawing = false;

    public enum PointerPos { left, center, right};
    public PointerPos pointerPos;
    private Vector3 position;
    private Vector3 previousPosition;

    public GameObject particle;
    public cartController cart;
    private float calculatedSpeed;
    

	void Start () {
        centerX = transform.position.x;
        pointerPos = PointerPos.center;

        particle = Instantiate(particle, transform.position, Quaternion.identity);
        particle.SetActive(false);
        Debug.Log("height: " + transform.lossyScale.y); // 1.7 -> need this
	}

	// Update is called once per frame
	void Update () {
        //TODO keep it before camera + moving down slowly -> then dissapear!
        //car need to spawn new, when reached the new waypoint or something...

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), Time.deltaTime * 0.1f);

        //test: simulate the drawing:
        if (drawing)
        {
            cart.speed = calculateDrawningSpeed();
        }
        else
        {
            cart.speed = 0;
        }
	}

    private float calculateDrawningSpeed()
    {
        if(previousPosition != null && position != null)
        {
            if(position.y - previousPosition.y > 0)
            {
                return position.y - previousPosition.y;
            }
            else
            {
                //user drawing down
                return 0;
            }
        }
        else
        {
            return Random.Range(0.1f, 2);
        }
    }
    
    public void startDrawing(BaseEventData e)
    {
        drawing = true;
        PointerEventData pointerData = e as PointerEventData;
        position = pointerData.pointerCurrentRaycast.worldPosition;
        Debug.Log("drawing started");
        particle.SetActive(true);
    }

    public void pointerDrawing(BaseEventData e)
    {
        PointerEventData pointerData = e as PointerEventData;
        position = pointerData.pointerCurrentRaycast.worldPosition;
        particle.SetActive(true);
        particle.transform.position = position;
        //TODO fix the position updated +  check the events calls on vr
        if (position != null)
        {     
            if (position.x > centerX)
            {
                pointerPos = PointerPos.left;
            }
            else if (position.y < centerX)
            {
                pointerPos = PointerPos.right;
            }
            else
            {
                pointerPos = PointerPos.center;
            }
        }
        previousPosition = position;
    }

    public void pointerOut()
    {
        if (drawing)
        {
            maxFail--;
            Debug.Log("Oops, u missed ->" + pointerPos);
            particle.SetActive(false);
            if (pointerPos == PointerPos.left)
            {
                cart.addExtraRotation(-30);
            }
            else if (pointerPos == PointerPos.right)
            {
                cart.addExtraRotation(30);
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
        Debug.Log("Drawing stopped");
        particle.SetActive(false);
    }
}

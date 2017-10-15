using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Shape") && other is SphereCollider)
        {
            if(other.gameObject.GetComponent<RailDrawer>() != null)
            {
                other.gameObject.GetComponent<RailDrawer>().checkPointCompleted(other);
            }
        }
    }
}

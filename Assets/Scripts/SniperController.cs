using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : GunController {

    private Animation shoot;
    private AudioSource shootAudio;

    //in seconds
    public float timeBetweenAttack = 2;
    private bool attackEnable = true;
    
    void Start () {
        shoot = GetComponent<Animation>();
        shootAudio = GetComponent<AudioSource>();
        
    }

    void Update () {
        
    }

    public override bool Shoot()
    {
        if (attackEnable)
        {
            StartCoroutine(attackDelay());
            if(shootAudio != null)
                shootAudio.Play();
            //here need to add some rotation
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator attackDelay()
    {
        attackEnable = false;
        yield return new WaitForSeconds(timeBetweenAttack);
        attackEnable = true;
    }
}

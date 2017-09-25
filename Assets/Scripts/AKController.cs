using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKController : GunController {

    private Animation shoot;
    private AudioSource shootAudio;

    void Start()
    {
        shoot = GetComponent<Animation>();
        //shootAudio = GetComponent<AudioSource>();
    }

    void Update () {
		
	}

    public override void Shoot()
    {
        shoot.Play("AKShooting");
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;                                 
    public Image damageImage;
    public float flashSpeed = 5f;                               
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    
    private bool isDead;                                                
    public bool damaged;
    public float attackRange = 20;

    private CamaraController movement;
    public GameObject weapon;
    public ParticleSystem dust;

    private GameObject hidedWeapon;
    private Quaternion previosRotation;
    public float maxRotation = 10;

    public GameObject restart;

    void Start () {
        currentHealth = startingHealth;
        movement = GetComponent<CamaraController>();
	}
	
	void Update () {
        
        if ((Input.GetMouseButtonDown(0) || GvrControllerInput.ClickButton) && weapon != null)
        {
            if (weapon.GetComponent<GunController>().Shoot())
            {
                RaycastHit hit;
                Ray weaponRay = new Ray(weapon.transform.GetChild(3).transform.position, chooseWeaponDirection());
                if (Physics.Raycast(weaponRay, out hit, attackRange))
                {
                    try
                    {
                        if (hit.collider.tag.Equals("ZombieHead"))
                        {
                            hit.transform.GetComponent<ZombieController>().takeDamage(100);
                        }
                        if (hit.collider.tag.Equals("ZombieBody"))
                        {
                            hit.transform.GetComponent<ZombieController>().takeDamage(20);
                        }
                        else
                        {
                            ParticleSystem dustObj = Instantiate(dust, hit.point, Quaternion.identity);
                            Destroy(dustObj.gameObject, 2);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        //sometimes its ok, dont care
                    }
                }
            }
        }
        if (weapon != null && weapon.GetComponent<GunController>() is AKController)
        {
            if(previosRotation != null)
            {
                Quaternion currentRotation = GvrControllerInput.Orientation;
                if((Math.Abs(previosRotation.eulerAngles.x - currentRotation.eulerAngles.x) < maxRotation && 
                    Math.Abs(previosRotation.eulerAngles.y - currentRotation.eulerAngles.y) < maxRotation && 
                        Math.Abs(previosRotation.eulerAngles.z - currentRotation.eulerAngles.z) < maxRotation))
                {
                    weapon.transform.localRotation = GvrControllerInput.Orientation;
                }
            }
            previosRotation = GvrControllerInput.Orientation;
        }

        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
	}

    private Vector3 chooseWeaponDirection()
    {
        if(weapon != null || weapon.GetComponent<GunController>() == null)
        {
            if(weapon.GetComponent<GunController>() is AKController)
            {
                //in this case the controller rotate our weapon
                return weapon.transform.forward;
            }
            if(weapon.GetComponent<GunController>() is SniperController)
            {
                //there we get the rotation from our camera
                return transform.root.GetChild(0).forward;
            }
        }
        Debug.LogError("Opps , can't find any weapon!");
        return Vector3.zero;
    }
    

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void hideWeapon(bool hide)
    {
        if(weapon != null)
        {
            if (hide)
            {
                hidedWeapon = weapon;
                weapon = null;
            }
            else if(hidedWeapon != null)
            {
                weapon = hidedWeapon;
                hidedWeapon = null;
            }
        }
        else
        {
            Debug.LogError("Can't hide weapon, because i don't have");
        }
    }


    private void Death()
    {
        isDead = true;
        movement.Wait = true;
        StartCoroutine(dying());
        GetComponent<Fading>().BeginFade(1);
        //restart.SetActive(true);
    }

    private IEnumerator dying()
    {
        for (int i = 1; i < 90; i++)
        {
            transform.Rotate(new Vector3(-1, 0, 0));
            yield return null;
        }
    }


    //only test call, i need more checkpoints!
    public void restartTestCall()
    {
        currentHealth = 100;
        GetComponent<CamaraController>().Wait = false;
        //restart.SetActive(false);
    }
}

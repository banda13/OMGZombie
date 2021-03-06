﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;                                 
    public Image damageImage;
    public float flashSpeed = 5f;                               
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public RawImage injurImage;
    
    public bool isDead;                                                
    public bool damaged;
    public float attackRange = 20;

    private CamaraController movement;
    public GameObject weapon;
    public GameObject weaponObject;
    public ParticleSystem dust;
    public AudioController audioController;

    private GameObject hidedWeapon;
    private Quaternion previosRotation;

    public CandleRespawning restart;

    public float maxRotationForceAtLowFearLevel = .5f;
    public float maxRotationForceAtNormalFearLevel = 1;
    public float maxRotationForceAtHighFearLevel = 2;

    public float currectRotationForce = .5f;
    
    //countans low, mid and fast audio sources, order is important
    public List<AudioSource> heartBeats;

    private enum heartBeat { slow, mid, fast };

    private heartBeat HeartBeat = heartBeat.mid;
    private bool isPaused = false;

    void Start () {
        currentHealth = startingHealth;
        movement = GetComponent<CamaraController>();

        AdaptedController.lowFearLevel += setLowFearLevelForce;
        AdaptedController.normalFearLevel += setNormalFearLevelForce;
        AdaptedController.highFearLevel += setHighFearLevelForce;

        AdaptedController.lowHeartRate += setLowHeartRate;
        AdaptedController.normalFearLevel += setNormalHeartRate;
        AdaptedController.highHeartRate += setFastHeartRate;
        
	}
	
	void Update () {
        //attacking
        if (isPaused)
        {
            return;
        }
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
                            findZombieParent(hit.collider.transform.gameObject).takeDamage(100);
                        }
                        if (hit.collider.tag.Equals("ZombieBody"))
                        {
                            findZombieParent(hit.collider.transform.gameObject).takeDamage(20);
                        }
                        if (hit.collider.tag.Equals("ExplosiveStuff"))
                        {
                            hit.collider.transform.gameObject.GetComponent<Detonator>().Explode();
                        }
                        else
                        {
                            ParticleSystem dustObj = Instantiate(dust, hit.point, Quaternion.identity);
                            Destroy(dustObj.gameObject, 2);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.LogError("Its ok, hit doesn't have zombie controller");
                    }
                }
            }
        }
        //weapon
        if (weapon != null && weapon.GetComponent<GunController>() is AKController)
        {
            weapon.transform.localRotation = GvrControllerInput.Orientation;
        }
        //damage image
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;

        //heartBeat
        if (HeartBeat == heartBeat.slow && heartBeats.Count > 0)
        {
            setUpHeartBeatRate(heartBeats[0]);
        }
        else if (HeartBeat == heartBeat.mid && heartBeats.Count > 1)
        {
            setUpHeartBeatRate(heartBeats[1]);
        }
        else if (HeartBeat == heartBeat.fast && heartBeats.Count > 2)
        {
            setUpHeartBeatRate(heartBeats[2]);
        }


        //healt
        if (currentHealth != 0 && currentHealth < 100)
        {
            currentHealth += Time.deltaTime * 1.1f;
        }

        if(currentHealth > 0)
        {
            injurImage.color = new Color(injurImage.color.r, injurImage.color.g, injurImage.color.b, 1 - (currentHealth / 100));
        }
        else if(currentHealth < 0)
        {
            injurImage.color = new Color(injurImage.color.r, injurImage.color.g, injurImage.color.b, 0);
        }
        else
        {
            injurImage.color = new Color(injurImage.color.r, injurImage.color.g, injurImage.color.b, 1);
        }
        
	}

    private void setUpHeartBeatRate(AudioSource current)
    {
        if (isDead)
        {
            return;
        }
        for(int i= 0; i < heartBeats.Count; i++)
        {
            if(heartBeats[i] == current)
            {
                if (!heartBeats[i].isPlaying)
                {
                    heartBeats[i].Play();
                }
            }
            else
            {
                heartBeats[i].Stop();
            }
        }
    }

    private void setLowFearLevelForce()
    {
        currectRotationForce = maxRotationForceAtLowFearLevel;
    }

    private void setNormalFearLevelForce()
    {
        currectRotationForce = maxRotationForceAtNormalFearLevel;
    }

    private void setHighFearLevelForce()
    {
        currectRotationForce = maxRotationForceAtHighFearLevel;
    }

    private void setLowHeartRate()
    {
        HeartBeat = heartBeat.slow;
    }

    private void setNormalHeartRate()
    {
        HeartBeat = heartBeat.mid;
    }
    private void setFastHeartRate()
    {
        HeartBeat = heartBeat.fast;
    }

    private ZombieController findZombieParent(GameObject child)
    {
        Transform t = child.transform;
        while(t != null)
        {
            if(t.tag == "Zombie")
            {
                return t.GetComponent<ZombieController>();
            }
            t = t.parent.transform;
        }
        return null;
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
        AdaptedEventHandler.playerDamaged(amount, currentHealth);
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void hideWeapon(bool hide)
    {
        try
        {
            if (hide)
            {
                hidedWeapon = weapon;
                weapon.SetActive(false);
                weapon = null;
            }
            else if (hidedWeapon != null)
            {
                weapon = hidedWeapon;
                weapon.SetActive(true);
                hidedWeapon = null;
            }
            else
            {
                GameObject wp = Instantiate(weaponObject, new Vector3(0, 0, 0), transform.rotation) as GameObject;
                wp.transform.parent = transform;
                wp.transform.localPosition = new Vector3(0.5f, -0.6f, 0.2f);
                wp.transform.rotation = transform.GetChild(0).transform.rotation;
                weapon = wp;
                weapon.SetActive(true);
                transform.GetChild(5).gameObject.SetActive(true);
            }
        } catch(Exception e)
        {
            Debug.Log("Could not hide weapon, cause isnt exist");
        }

    }


    private void Death()
    {
        isDead = true;
        movement.Wait = true;
        AdaptedEventHandler.playerDead(movement.getCurrentWaypointPosition(), movement.getCurrentWaypointName());
        StartCoroutine(dying());
        StartCoroutine(restart.delayedJump(7));
        StartCoroutine(audioController.turnDownMusic());
        StartCoroutine(turnDownHeart(2));
        //restart.SetActive(true);
    }

    private IEnumerator dying()
    {
        for (int i = 1; i < 90; i++)
        {
            transform.Rotate(new Vector3(-1, 0, 0));
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            yield return null;
        }
        GetComponent<Fading>().BeginFade(1);
    }

    public IEnumerator turnDownHeart(int value)
    {
        for(int i = 0; i < heartBeats.Count; i++)
        {
            if(i == value)
            {
                heartBeats[i].Play();
            }
            else
            {
                heartBeats[i].Stop();
            }
        }
        yield return new WaitForSeconds(1);
        if(value != -1)
        {
            StartCoroutine(turnDownHeart(value--));
        }
    }

    //only test call, i need more checkpoints!
    public void restartTestCall()
    {
        currentHealth = 100;
        GetComponent<CamaraController>().Wait = false;
        //restart.SetActive(false);
    }

    void OnPauseGame()
    {
        isPaused = true;
        if (weapon != null && weapon.GetComponent<GunController>() is AKController)
        {
            hidedWeapon = weapon;
            weapon.SetActive(false);
            weapon = null;
        }
    }

    void OnResumeGame()
    {
        isPaused = false;
        if (hidedWeapon != null && hidedWeapon.GetComponent<GunController>() is AKController)
        {
            weapon = hidedWeapon;
            weapon.SetActive(true);
            hidedWeapon = null;
        }
    }
}

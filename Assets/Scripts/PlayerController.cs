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
    
    void Start () {
        currentHealth = startingHealth;
	}
	
	void Update () {

        if (Input.GetMouseButtonDown(0) && weapon != null)
        {
            RaycastHit hit;
            Ray weaponRay = new Ray(weapon.transform.GetChild(3).transform.position, transform.GetChild(0).forward);
            if(Physics.Raycast(weaponRay, out hit, attackRange))
            {
                if (hit.collider.tag.Equals("Zombie"))
                {
                    hit.collider.gameObject.GetComponent<ZombieController>().Die();
                }
            }
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


    void Death()
    {
        isDead = true;
        movement.Wait = true;
    }
}

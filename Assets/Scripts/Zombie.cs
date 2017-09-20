using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Zombie : MonoBehaviour {

    public GameObject Player;
    public Animator animator;
    public int number;
    
    public float speed = 0;
    public bool targetReached { get; set; }

    public int damage = 10;
    
    public enum States {spawn, walking, attack, die};

    protected States state { get; set; }
    
    private Vector3 destinationPosition;
    public GameObject sphere;

    protected void init()
    {
        animator = GetComponent<Animator>();
        state = States.spawn;
    }

    protected void startWalking()
    {
        state = States.walking;
        speed = 0.5f;
        updateAnimator();
        targetReached = true;
    }
    
    protected void die()
    {
        animator.SetTrigger("Die");
        state = States.die;
    }

    protected void goToDestination()
    {
		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
			return;
		}
        else
        {
            if (positionReached(transform.position, destinationPosition, 0.3f))
            {
                targetReached = true;
            }
        }
    }
    protected bool rotateToDirection(float rotationSpeed)
    {
        Vector3 degree = Vector3.Cross(transform.forward, destinationPosition - transform.position);
        if (Mathf.Abs(degree.y) < 0.1)
        {
            speed = 0.5f;
            updateAnimator();
            return true;
        }
        else
        { 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destinationPosition), rotationSpeed * Time.deltaTime);
            return false;
        }
    }
    protected void chooseDestination(GameObject lunch)
    {
        if(lunch == null)
        {
            destinationPosition = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-3, 3));
        }
        else
        {
            destinationPosition = new Vector3(lunch.transform.position.x, 0, lunch.transform.position.z);

        }
        Instantiate(sphere, destinationPosition, Quaternion.identity);
        targetReached = false;
    }

    private bool positionReached(Vector3 a, Vector3 b, float distance)
    {
        if(Vector3.Distance(a , b) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool isPlayerNeerOrWeSawHim()
    {
        if (positionReached(Player.transform.position, transform.position, 7))
        {
            return true;
        }
        else
        {   
            if(state == States.attack)
            {
                animator.SetBool("attack", false);
                speed = 0.9f;
                startWalking();
            }
            return false;
        }
    }

    protected void attackPlayer()
    {
        state = States.attack;
        if(positionReached(Player.transform.position, transform.position, 1.3f))
        {
            Debug.Log("Zombie: " + number + " attacked! Damaged: " + damage);
            attack();
        }
        else
        {
            chooseDestination(Player);
            if (rotateToDirection(300)) {
                speed = 0.9f;
                updateAnimator();
                goToDestination();
            }
        }
    }

    private void updateAnimator()
    {
        animator.SetFloat("Speed", speed);
    }

    private void attack()
    {
        Player.GetComponent<PlayerController>().TakeDamage(damage);
        animator.SetBool("Attack", true);
    }
}

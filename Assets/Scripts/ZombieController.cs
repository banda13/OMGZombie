using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{

    private bool spawned = false;
    private bool dead = false;
    private int zombieIndex;

    public Animator animator;
    public GameObject player;
    private Vector3 previousPosition;
    private Vector3 destination;
    private CharacterController controller;
    public GameObject sphere;

    public float speed = 0;
    public float directionChange = 4;
    public float maxRotation = 90;
    public int healt = 100;
    public int attackDamage = 10;
    public float attackRange = 1.8f;
    public float attackSpeed = 1;
    public float eyeShot = 10;
    
    private bool targetReached = true;
    private float attackTimer;


    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        attackTimer = 0;
    }
    
    void Update () {
        
        if (!Spawned() || dead)
        {
            return;
        }
        if (canAttack())
        {
            Attack();
        }
        Move();

        attackTimer += Time.deltaTime;
    }


    //interaction with the zombie= die, and spawn
    public void Die()
    {
        dead = true;
        animator.SetInteger("Die", 1);
    }

    private bool Spawned()
    {
        if (spawned)
        {
            return true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        { 
            animator.runtimeAnimatorController = Resources.Load(Paths.normalBehaviorController) as RuntimeAnimatorController;
            controller = transform.gameObject.AddComponent(typeof(CharacterController)) as CharacterController;
            controller.center = new Vector3(0, 1, 0);
            controller.radius = 0.4f;
            //StartCoroutine(NewHeading());
            spawned = true;
        }
        return false;
    }

    public void Spawn(int index)
    {
        zombieIndex = index;
        targetReached = true;
    }

    //zombie interactions
    private void Attack()
    {
        //Debug.Log("Zombie" + zombieIndex + " is attacking!");
        player.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }

    private void Move()
    {
        if (detectPlayer() && rotateToDestination(300, new Vector3(player.transform.position.x, 0, player.transform.position.z)))
        {
            speed = 0.9f;
        }
        else if (targetReached || zombieStuck())
        {
            changeDestination();
        }
        else if (rotateToDestination(100, destination))
        {
            speed = 0.6f;
        }


        animator.SetBool("Attack", false);
        animator.SetFloat("Speed", speed);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        controller.Move(transform.forward * speed * 0.01f);

        //positioning fixes to avoid flying zombies!

        if (isDistanceSmaller(transform.position, destination, 0.1f))
        {
            Debug.Log("Target reached: " + destination);
            targetReached = true;
        }
    }

    private void changeDestination()
    {
        destination = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-3, 3));
        targetReached = false;
        if (sphere != null)
        {
            Instantiate(sphere, destination, Quaternion.identity);
        }
        //StopCoroutine(NewHeading());
        //StartCoroutine(NewHeading());
        //Debug.Log("Zombie " + zombieIndex + " choosed new destination: " + destination);
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            changeDestination();
            yield return new WaitForSeconds(directionChange);
        }
    }
    
    private bool canAttack()
    {
        if(isDistanceSmaller(transform.position, player.transform.position, attackRange) && attackTimer >= attackSpeed)
        {
            animator.SetBool("Attack", true);
            //Debug.Log("Zombie" + zombieIndex + "is attacking");
            attackTimer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //helpers to the zombie interactions
    private bool isDistanceSmaller(Vector3 a, Vector3 b, float distance)
    {
        if (Vector3.Distance(a, b) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //rotationSpeed based on the zombie state(attacking or just wandering)
    private bool rotateToDestination(float rotationSpeed, Vector3 destination)
    {
        Vector3 degree = Vector3.Cross(transform.forward, destination - transform.position);
        if (Mathf.Abs(degree.y) < 0.1)
        {
            return true;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination- transform.position), rotationSpeed * Time.deltaTime);
            return false;
        }
    }

    private bool playerBeforeZombie()
    {
        //Debug.DrawRay(transform.position, transform.forward, Color.red, eyeShot);
        return false;
    }

    private bool zombieStuck()
    {
        //have we move since our last know destination? 
        //is there any obstacle before us?
        return false;
    }

    private bool detectPlayer()
    {
        if(isDistanceSmaller(player.transform.position, transform.position, 8.0f) || playerBeforeZombie())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

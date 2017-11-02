using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{

    private bool spawned = false;
    private bool dead = false;
    public int zombieIndex;
    private bool isPaused = false;

    public Animator animator;
    public GameObject player;
    private Vector3 previousPosition;
    public Vector3 destination;
    private Vector3 nextDestination;
    private CharacterController controller;
    public GameObject sphere;
    public PhysicMaterial zerofriction;

    public float speed = 0;
    public float directionChange = 4;
    public int healt = 100;
    public int attackDamage = 10;
    public int biteDamage = 20;
    public float attackRange = 1.8f;
    public float attackSpeed = 1;
    public float eyeShot = 10;
    public float playerDetectionRange = 8.0f;
    public float obstacleDetection = 1.5f;

    private bool targetReached = true;
    private float attackTimer;

    private bool playerDetected = false;
    private bool attackEnable = false;
    private bool dying = false;

    public float speed_animationIdle = 0.0f;
    public float speed_animationInjured = 0.3f;
    public float speed_animationWalk = 0.6f;
    public float speed_animationRun = 0.9f;

    public AudioSource spawningAudio;
    public AudioSource randomAudio;
    public AudioSource randomAudio2;
    public AudioSource playerDetectionAudio;
    public AudioSource attackAudio;
    public AudioSource biteAudio;
    public AudioSource injuringAudio;
    public AudioSource injuringAudio2;
    public AudioSource dyingAudio;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        //controller = GetComponent<CharacterController>();
        attackTimer = 0;

        spawningAudio = addAudioSouce(Paths.spawningAudio);
        randomAudio = addAudioSouce(Paths.randomAudio);
        randomAudio2 = addAudioSouce(Paths.randomAudio2);
        playerDetectionAudio = addAudioSouce(Paths.detectionAudio);
        attackAudio = addAudioSouce(Paths.attackAudio);
        biteAudio = addAudioSouce(Paths.biteAudio);
        injuringAudio = addAudioSouce(Paths.injuringAudio1);
        injuringAudio2 = addAudioSouce(Paths.injuringAudio2);
        dyingAudio = addAudioSouce(Paths.dieAudio);
    }
    

    void Update () {
        if (isPaused)
        {
            return;
        }
        if (!Spawned() || dead)
        {
            if (dying)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -0.9f, transform.position.z), Time.deltaTime);
            }
            return;
        }
        if (!Attack())
            Move();
        
        attackTimer += Time.deltaTime;
    }

    private AudioSource addAudioSouce(string clipUrl)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = Resources.Load(clipUrl) as AudioClip;
        audio.volume = 0.9f;
        audio.spatialBlend = 1;
        audio.maxDistance = 5;
        return audio;
    }
    
    public IEnumerator Die()
    {
        dead = true;
        speed = 0;
        animator.SetInteger("Die", 1);
        dyingAudio.Play();
        yield return new WaitForSeconds(1);
        dying = true;
    }
    
    public void setNextDestination(Transform dest)
    {
        nextDestination = dest.position;
    }

    private IEnumerator waitSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
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
            Rigidbody rb = transform.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            CapsuleCollider collider = transform.gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            collider.center = new Vector3(0, 1, 0);
            collider.height = 2.0f;
            collider.material = zerofriction;
            spawned = true;
            spawningAudio.Play();
            StartCoroutine(randomVoice());
        }
        return false;
    }

    private void Move()
    {
        if ((detectPlayer() && rotateToDestination(300, new Vector3(player.transform.position.x, 0, player.transform.position.z))))
        {
            if (!playerDetected)
            {
                StartCoroutine(playerDetection());
            }else
            {
                playerDetected = true;
            } 
            if(healt < 50)
            {
                speed = speed_animationInjured;
            }
            else
            {
                speed = speed_animationRun;
            }
            
        }
        else if (targetReached || zombieStuck())
        {
            changeDestination();
        }
        else if (rotateToDestination(100, destination))
        {
            if (healt < 50)
            {
                speed = speed_animationInjured;
            }
            else
            {
                speed = speed_animationWalk;
            }
        }
        
        animator.SetFloat("Speed", speed);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        //controller.Move(transform.forward * speed * 0.01f);

        //positioning fixes to avoid flying zombies!

       // transform.position = new Vector3(transform.position.x, 0, transform.position.z);
       // transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

        if (isDistanceSmaller(transform.position, destination, 0.1f))
        {
            //Debug.Log("Target reached: " + destination);
            targetReached = true;
        }
    }

    private IEnumerator randomVoice()
    {
        int random = Random.Range(5, 15);
        yield return new WaitForSeconds(random);
        AudioSource r = Random.Range(0, 100) > 50 ? randomAudio : randomAudio2;
        r.Play();
        StartCoroutine(randomVoice());
    }

    private IEnumerator playerDetection()
    {
        playerDetectionAudio.Play();
        yield return new WaitForSeconds(Random.Range(0, 5));
        playerDetected = false;
    }

    private void changeDestination()
    {
        if(nextDestination != null && nextDestination != Vector3.zero)
        {
            destination = nextDestination;
        }
        else
        {
            destination = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-3, 3));
            StartCoroutine(NewHeading());
        }
        targetReached = false;
        if (sphere != null)
        {
            Instantiate(sphere, destination, Quaternion.identity);
        }
        //StopCoroutine(NewHeading());
        //Debug.Log("Zombie " + zombieIndex + " choosed new destination: " + destination);
    }

    IEnumerator NewHeading()
    {
        yield return new WaitForSeconds(directionChange);
        changeDestination();
        Debug.Log("Zombie"+zombieIndex+": Time is up, new destination choosed");
    }
    
    private bool Attack()
    {
        if(player == null)
        {
            Debug.LogError("Can't find the player component");
        }
        if(isDistanceSmaller(transform.position, player.transform.position, attackRange) && attackTimer >= attackSpeed && attackEnable)
        {
            if(Random.Range(0, 100) > 70)
            {
                animator.SetBool("Bite", true);
                player.GetComponent<PlayerController>().TakeDamage(biteDamage);
                biteAudio.Play();
            }
            else
            {
                animator.SetBool("Attack", true);
                player.GetComponent<PlayerController>().TakeDamage(attackDamage);
                attackAudio.Play();
            }
            //Debug.Log("Zombie" + zombieIndex + "is attacking");
            attackTimer = 0;
            return true;
        }
        else
        {
            animator.SetBool("Bite", false);
            animator.SetBool("Attack", false);
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
        if (Mathf.Abs(degree.y) < 0.2) 
        {
            return true;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination- transform.position, Vector3.up), rotationSpeed * Time.deltaTime);
            return false;
        }
    }

    private bool playerBeforeZombie()
    {
        RaycastHit hit;
        Ray eye = new Ray(transform.position+ new Vector3(0, 1.6f, 0), transform.forward);

        //Debug.DrawRay(transform.position + new Vector3(0, 1.6f, 0), transform.forward, Color.red, eyeShot);
        if (Physics.Raycast(eye, out hit, eyeShot))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                //Debug.Log("Zombie" + zombieIndex + ": The lunch is before me. Distance :" + Vector3.Distance(transform.position, new Vector3(hit.point.x, 0, hit.point.z)));
                return true;
            }
        }
        return false;
    }

    public void takeDamage(int damage)
    {
        healt -= damage;
        animator.SetTrigger("Hit");
        AudioSource r = Random.Range(0, 100) > 50 ? injuringAudio : injuringAudio2;
        r.Play();
        if(healt <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private bool zombieStuck()
    {
        
        //have we move since our last know destination? 
        //is there any obstacle before us?
        RaycastHit hit;
        Ray eye = new Ray(transform.position + new Vector3(0, 1.5f, 0), destination-transform.position);
        if (Physics.Raycast(eye, out hit, obstacleDetection))
        {
            if (!hit.collider.tag.Equals("Player") && !hit.collider.tag.Equals("Zombie"))
            {
                //Debug.Log("Zombie " + zombieIndex + ": Oops a " + hit.collider.name + " is before me, I'll change my destination");
                //Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), destination - transform.position, Color.green, obstacleDetection);
                return true;
            }
        }
        return false;
    }

    private bool detectPlayer()
    {
        if (!attackEnable)
        {
            return false;
        }
        if(isDistanceSmaller(player.transform.position, transform.position, playerDetectionRange) || playerBeforeZombie())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void enableAttack(bool enable)
    {
        attackEnable = enable;
    }

    public bool isDead()
    {
        return dead;
    }
    void OnPauseGame()
    {
        isPaused = true;
    }

    void OnResumeGame()
    {
        isPaused = false;
    }
}

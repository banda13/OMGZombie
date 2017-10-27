using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStandingBehavior : MonoBehaviour {

    public Animator animator;
    public float timeBetweenActions = 5;
    private bool spawned = false;
    private bool dead = false;
    private bool activated = false;
    
    public List<string> animatorTriggers;
    private int animationCounter = 0;
    public GameObject player;

	void Start () {
        animator = GetComponent<Animator>();
	}

    void Update() {

        Spawned();
        if (!activated && Vector3.Distance(transform.position, player.transform.position) < 12)
        {
            Debug.Log("Spawning started");
            animator.SetBool("Block", false);
            activated = true;
        }
	}

    private bool Spawned()
    {
        if (spawned)
        {
            return true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.runtimeAnimatorController = Resources.Load(Paths.notMovingZombieController) as RuntimeAnimatorController;
            Rigidbody rb = transform.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            CapsuleCollider collider = transform.gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            collider.center = new Vector3(0, 1, 0);
            collider.height = 2.0f;
            //collider.material = zerofriction;
            spawned = true;
            StartCoroutine(startDoingSomething());

        }
        return false;
    }


    private IEnumerator startDoingSomething()
    {
        Debug.Log("Action " + animatorTriggers[animationCounter] + " started");
        animator.SetTrigger(animatorTriggers[animationCounter]);
        animationCounter = (animationCounter == animatorTriggers.Count-1  ? 0 : animationCounter+1 );
        yield return new WaitForSeconds(timeBetweenActions);
        StartCoroutine(startDoingSomething());
    }
}

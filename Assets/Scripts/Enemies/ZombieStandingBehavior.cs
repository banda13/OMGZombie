using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStandingBehavior : MonoBehaviour {

    public Animator animator;
    public float timeBetweenActions = 5;
    private bool spawned = false;
    private bool dead = false;
    private bool activated = false;
    private bool isPaused = false;

    public List<string> animatorTriggers;
    private int animationCounter = 0;
    public GameObject player;

    public float activationDistence  = 12;

    //important to has every animation an audio => animationTriggers.Count == audios.Count
    private List<AudioSource> audios;

	void Start () {
        animator = GetComponent<Animator>();
        audios = new List<AudioSource>();
        audios.Add(addAudioSouce(Paths.screamAudio));
        audios.Add(addAudioSouce(Paths.attackAudio));

	}

    void Update() {
        if (isPaused)
        {
            return;
        }

        if (Spawned())
        {
            Quaternion q = Quaternion.LookRotation(new Vector3(player.transform.position.x, 0, player.transform.position.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 30 *Time.deltaTime);
        }
        if (!activated && Vector3.Distance(transform.position, player.transform.position) < activationDistence)
        {
            animator.SetBool("Block", false);
            activated = true;
        }
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
        try
        {
            animator.SetTrigger(animatorTriggers[animationCounter]);
            audios[animationCounter].Play();
            animationCounter = (animationCounter == animatorTriggers.Count - 1 ? 0 : animationCounter + 1);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.Log("animationTriggers.Count == audios.Count !!!!!");
        }
        yield return new WaitForSeconds(timeBetweenActions);
        StartCoroutine(startDoingSomething());
        
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

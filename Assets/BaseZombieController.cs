using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseZombieController : MonoBehaviour {

    public bool spawned = false;
    protected bool dead = false;
    private int zombieIndex;

    public Animator animator;
    public GameObject player;
    protected Vector3 destination;
    protected CharacterController controller;

    public float speed = 0;
    public float directionChange = 2;
    public int damage = 10;

    private bool targetReached = true;

    void Start () {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
	
	void Update () {
		
	}

    protected abstract void Attack();

    protected abstract bool canAttack();

    protected abstract void Move();

    protected abstract bool canMove();

    protected abstract void Die();
}

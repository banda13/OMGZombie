using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : BaseZombieController
{
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        if (!spawned || dead)
        {
            return;
        }
        if (canAttack())
        {
            Attack();
        }
        else if (canMove())
        {
            Move();
        }
        
	}

    protected override void Attack()
    {
        throw new NotImplementedException();
    }

    protected override void Die()
    {
        dead = true;
        animator.SetInteger("Die", 1);
    }

    protected override void Move()
    {
        throw new NotImplementedException();
    }

    protected override bool canAttack()
    {
        return false;
    }

    protected override bool canMove()
    {
        throw new NotImplementedException();
    }
}

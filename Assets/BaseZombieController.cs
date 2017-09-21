using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseZombieController : MonoBehaviour {

   

   

    protected abstract void Attack();

    protected abstract bool canAttack();

    protected abstract void Move();

    protected abstract bool canMove();

    protected abstract void Die();

    
}

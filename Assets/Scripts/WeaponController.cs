using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject sniper;
    private Animator animator;
    public GameObject sniperView;

    private bool isScoped = false;
    public GameObject weaponCamera;
    public Camera mainCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    public bool Activated = false;

    public float aimDisturbForce = 1;
    public float aimDisturbTime = 1;
    public float timeBetweenAttack = 2;
    private bool newDisturbDirection = true;
    private Quaternion destRot;


    void Start () {
        animator = GetComponent<Animator>();
	}
	
	void Update () {

        transform.forward = mainCamera.transform.forward;
        if (Activated && Input.GetKeyDown("space"))
        {
            isScoped = !isScoped;
            animator.SetBool("isScoped", isScoped);
            if (isScoped)
            {
                StartCoroutine(OnScoped());
            }
            else
            {
                StartCoroutine(OnUnScoped());
            }
        }
        if(Activated && isScoped)
        {
            disturb();
        }
	}

    private void disturb()
    {
        if (newDisturbDirection)
        {
            StartCoroutine(chooseDisturbDirection());
        }
       
        transform.root.rotation = Quaternion.RotateTowards(transform.root.rotation, destRot, aimDisturbTime * Time.deltaTime);
    }

    private IEnumerator chooseDisturbDirection()
    {
        float randomX = Random.Range(-aimDisturbForce, aimDisturbForce);
        float randomY = Random.Range(-aimDisturbForce, aimDisturbForce);
        destRot = transform.root.rotation * Quaternion.Euler(randomX, randomY, 0);
        newDisturbDirection = false;
        Debug.Log("New destination choosed");
        yield return new WaitForSeconds(2);
        newDisturbDirection = true;
    }

    public IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);
        sniperView.SetActive(true);
        weaponCamera.SetActive(false);

        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;

        newDisturbDirection = true;
    }

    public IEnumerator OnUnScoped()
    {
        yield return new WaitForSeconds(.15f);
        sniperView.SetActive(false);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
    }
}

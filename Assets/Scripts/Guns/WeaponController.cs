using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject sniper;
    private Animator animator;
    public GameObject sniperView;

    public bool isScoped = false;
    public GameObject weaponCamera;
    public Camera mainCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    public bool Activated = false;

    public float aimDisturbanceAtLowFearLevel = 10;
    public float aimDisturbanceAtNormalFearLevel = 15;
    public float aimDisturbanceAtHighFearLevel = 20;
    public float aimDisturbForce = 10;
    public float aimDisturbTime = 1;

    public float timeBetweenAttack = 2;
    private bool newDisturbDirection = true;
    private Quaternion destRot;


    void Start () {
        animator = GetComponent<Animator>();

        AdaptedFearController.lowFearLevel += setDisturbanceToLowLevel;
        AdaptedFearController.normalFearLevel += setDisturbanceToNormalLevel;
        AdaptedFearController.highFearLevel += setDisturbanceToHighLevel;
	}
	
	void Update () {

        transform.forward = mainCamera.transform.forward;
        transform.GetChild(0).GetChild(3).position = new Vector3(mainCamera.transform.position.x , mainCamera.transform.position.y, mainCamera.transform.position.z) + mainCamera.transform.forward * 1;
        transform.GetChild(0).GetChild(4).position = new Vector3(mainCamera.transform.position.x , mainCamera.transform.position.y, mainCamera.transform.position.z) + mainCamera.transform.forward * 1;
        if (sniperView.active)
        {
            sniperView.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1.0f;
            sniperView.transform.LookAt(mainCamera.transform.position);
        }

        if (Activated && (Input.GetKeyDown(KeyCode.UpArrow) || (GvrControllerInput.TouchDown)))
        {
            isScoped = true;
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScoped());
        }
        if(Activated && (Input.GetKeyDown(KeyCode.DownArrow) || (GvrControllerInput.TouchUp))){
            isScoped = false;
            animator.SetBool("isScoped", false);
            StartCoroutine(OnUnScoped());
        }
        if(Activated && isScoped)
        {
            disturb();
        }
	}

    private void setDisturbanceToLowLevel()
    {
        aimDisturbForce = aimDisturbanceAtLowFearLevel;
        aimDisturbTime = aimDisturbanceAtLowFearLevel / 10;
    }

    private void setDisturbanceToNormalLevel()
    {
        aimDisturbForce = aimDisturbanceAtNormalFearLevel;
        aimDisturbTime = aimDisturbanceAtNormalFearLevel / 10;
    }

    private void setDisturbanceToHighLevel()
    {
        aimDisturbForce = aimDisturbanceAtHighFearLevel;
        aimDisturbTime = aimDisturbanceAtHighFearLevel / 10;
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
        yield return new WaitForSeconds(2);
        newDisturbDirection = true;
    }

    public IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);
        sniperView.SetActive(true);
        weaponCamera.SetActive(false);

        //normalFOV = mainCamera.fieldOfView;
        //mainCamera.fieldOfView = scopedFOV;

        newDisturbDirection = true;
    }
    
    public IEnumerator OnUnScoped()
    {
        yield return new WaitForSeconds(.15f);
        sniperView.SetActive(false);
        weaponCamera.SetActive(true);
        //mainCamera.fieldOfView = normalFOV;
    }
}

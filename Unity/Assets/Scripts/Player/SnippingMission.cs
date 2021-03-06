﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnippingMission : PathFollower
{

    public GameObject weaponHolder;
    private WeaponController weapon;
    public GameObject dummyWeapon;
    public GameObject points;
    public EnemyFactory factory;
    public Transform destination;
    public GameObject StartMessage;
    public GameObject CompletedMessage;
    public GameObject FailedMessage;
    public ChestController chest;
    public AudioController audioController;

    private bool started = false;
    private bool playerStarted = false;
    private bool putUpGun = false;
    private bool ended = false;
    private bool failed = false;
    private bool playerAcceptWinning = false;
    private bool isPaused = false;
    public delegate void fadingActions();

    private List<Transform> spawningPoints;

    public int zombiesNumber = 5;
    public float timeBetweenSpawn = 10;

    private List<ZombieController> zombiesInGame;

    //for the test calls:
    public delegate void setBoolean();

    void Start()
    {
        shift = new Vector3(0, 0.5f, 0);
        init();
        Wait = true;
        weapon = weaponHolder.GetComponent<WeaponController>();

        spawningPoints = new List<Transform>();

        foreach (Transform point in points.transform)
        {
            spawningPoints.Add(point);
        }
        zombiesInGame = new List<ZombieController>();
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        move();

        if (currentWaypoint == waypoints.Count && !Wait)
        {
            Wait = true;
            //this.transform.root.GetComponent<CamaraController>().stopSnipeMission();
        }

        if (!started && !ended && waypoints[currentWaypoint].transform.name.Contains("battle"))
        {
            Wait = true;
            if (playerStarted)
            {
                if (putUpGun)
                {
                    audioController.isSnippingMission = true;
                    started = true;
                    weaponHolder.SetActive(true);
                    weapon.Activated = true;
                    dummyWeapon.SetActive(false);
                    transform.root.gameObject.GetComponent<PlayerController>().weapon = weaponHolder.transform.GetChild(0).transform.gameObject;
                    StartCoroutine(createZombies());
                    AdaptedEventHandler.missionStarted("Sniper mission");
                }
                //#if UNITY_EDITOR
                //                StartCoroutine(delayedTestCall(onClickOnGun));
                //#endif
            }
            else
            {
                StartMessage.SetActive(true);
                //#if UNITY_EDITOR
                //                StartCoroutine(delayedTestCall(startGame));
                //#endif
            }
        }
        if (!ended && checkMissionEnded() && (playerAcceptWinning || failed))
        {
            Ended();
        }

    }
    //it looks good
    IEnumerator delayedTestCall(setBoolean f)
    {
        yield return new WaitForSeconds(2);
        f();
    }

    public void startGame()
    {
        if (!isPaused)
        {
            playerStarted = true;
            StartMessage.SetActive(false);
        }
    }

    public void onClickOnGun()
    {
        if (playerStarted && !isPaused)
            putUpGun = true;
    }

    public void acceptWinning()
    {
        if (!isPaused)
        {
            playerAcceptWinning = true;
            CompletedMessage.SetActive(false);
        }
    }

    public void restartMission()
    {
        if (!isPaused)
        {
            Debug.Log("Mission will restart soon");
            factory.killAll();
            zombiesInGame = new List<ZombieController>();
            StartCoroutine(delayedTestCall(restartAfterZombiesKilled));
            FailedMessage.SetActive(false);
        }
    }

    private void restartAfterZombiesKilled()
    {
        if (!isPaused)
        {
            failed = false;
            transform.root.gameObject.GetComponent<PlayerController>().weapon = null;
            weaponHolder.SetActive(false);
            weapon.Activated = false;
            ended = false;
            putUpGun = false;
            started = false;
        }
    }

    private IEnumerator createZombies()
    {
        ZombieController zombie = factory.cleverZombieCreation(spawningPoints);
        zombiesInGame.Add(zombie);
        zombie.playerDetectionRange = 0;
        zombie.eyeShot = 0;
        zombie.setNextDestination(destination);
        zombie.directionChange = 120; // in this case the zombie probaly stuck somewhere
        setZombieSpeed(zombie);

        yield return new WaitForSeconds(timeBetweenSpawn);
        if (zombiesInGame.Count < zombiesNumber)
        {
            StartCoroutine(createZombies());
        }
    }
    //to make harder zombies, we set the walk animation speed of the zombies
    //they still think they walk, but actually they're running 
    private void setZombieSpeed(ZombieController zombie)
    {
        float current = zombiesInGame.Count;
        float limit = zombiesNumber;
        if (current < limit / 2)
        {
            zombie.speed_animationWalk = 0.5f;
        }
        else
        {
            zombie.speed_animationWalk = 0.9f;
        }
    }

    private IEnumerator startZombieIfStuck(ZombieController zombie)
    {
        yield return new WaitForSeconds(5);
        if (zombie.speed == 0 && !zombie.isDead())
        {
            zombie.speed = 0.8f;
        }
    }

    public void fadingAction()
    {
        factory.clearFactory();
        FailedMessage.SetActive(true);
        StartCoroutine(putDownTheGun());
        Debug.Log("Zombies reached the house..");
    }

    public bool checkMissionEnded()
    {
        bool noDeathButZombiesAlive = false;
        foreach (ZombieController enemies in zombiesInGame)
        {
            if (!enemies.isDead())
            {
                noDeathButZombiesAlive = true;
            }
            if (Vector3.Distance(enemies.transform.position, destination.position) < 2)
            {
                //Oh no, the zombies reached the house, u will die.. :'(

                failed = true;
                AdaptedEventHandler.missionFailed("Sniper mission", getKilledZombies());
                StartCoroutine(transform.root.gameObject.GetComponent<CamaraController>().fadingWithSniperAction(fadingAction));
                return true;
            }
        }
        if (noDeathButZombiesAlive)
        {
            return false;
        }
        else if (zombiesInGame.Count == zombiesNumber)
        {
            Debug.Log("Oke, u killed " + zombiesInGame.Count + " zombies, the mission was completed");
            StartCoroutine(putDownTheGun());
            CompletedMessage.SetActive(true);
            return true;
        }
        else
        {
            //its ok, then mission not started
            return false;
        }
    }

    private int getKilledZombies()
    {
        int count = 0;
        foreach (ZombieController enemies in zombiesInGame)
        {
            if (enemies.isDead())
            {
                count++;
            }
        }
        return count;
    }

    public override void Go()
    {
        Wait = false;
    }

    public void Ended()
    {
        audioController.isSnippingMission = false;
        if (!failed)
        {
            chest.missionCompleted = true;

            //important!!!
            //this activates the  zombie creation
            //you should not start it earlier, otherwise the player would have no change
            factory.Active = true;
            factory.setUpZombieVillage();
            Go();
            AdaptedEventHandler.missionCompleted("Sniper mission");
        }
        ended = true;

    }

    public IEnumerator putDownTheGun()
    {
        if (weapon.isScoped)
        {
            StartCoroutine(weapon.OnUnScoped());
        }
        yield return new WaitForSeconds(.2f);
        weaponHolder.SetActive(false);
        dummyWeapon.SetActive(true);
        GameObject w = transform.root.gameObject.GetComponent<PlayerController>().weapon;
        if (w != null && w.GetComponent<SniperController>())
        {
            transform.root.gameObject.GetComponent<PlayerController>().weapon = null;
        }
    }

    void OnPauseGame()
    {
        isPaused = true;
        StartCoroutine(putDownTheGun());
    }

    void OnResumeGame()
    {
        isPaused = false;
    }

}

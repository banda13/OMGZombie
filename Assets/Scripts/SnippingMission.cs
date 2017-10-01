using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnippingMission : PathFollower {

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

    private bool started = false;
    private bool playerStarted = false;
    private bool putUpGun = false;
    private bool ended = false;
    private bool failed = false;
    private bool playerAcceptWinning = false;

    private List<Transform> spawningPoints;

    public int zombiesNumber = 5;
    public float timeBetweenSpawn = 10;

    private List<ZombieController> zombiesInGame;

    //for the test calls:
    public delegate void setBoolean();

	void Start () {
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
	
	void Update () {

        move();

        if (currentWaypoint == waypoints.Count && !Wait)
        {
            Wait = true;
            this.transform.root.GetComponent<CamaraController>().stopSnipeMission();
        }

        if (!started && !ended && waypoints[currentWaypoint].transform.name.Contains("battle"))
        {
            Wait = true;
            if (playerStarted)
            {
                if (putUpGun)
                {
                    started = true;
                    weaponHolder.SetActive(true);
                    weapon.Activated = true;
                    dummyWeapon.SetActive(false);
                    transform.root.gameObject.GetComponent<PlayerController>().weapon = weaponHolder.transform.GetChild(0).transform.gameObject;
                    StartCoroutine(createZombies());
                }
                StartCoroutine(delayedTestCall(onClickOnGun));
            }
            else
            {
                StartMessage.SetActive(true);
                StartCoroutine(delayedTestCall(startGame));
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
        playerStarted = true;
        StartMessage.SetActive(false);
    }

    public void onClickOnGun()
    {
        putUpGun = true;
    }
    
    public void acceptWinning()
    {
        playerAcceptWinning = true;
    }

    private IEnumerator createZombies()
    {
        ZombieController zombie = factory.cleverZombieCreation(spawningPoints);
        zombiesInGame.Add(zombie);
        zombie.playerDetectionRange = 0;
        zombie.eyeShot = 0;
        zombie.setNextDestination(destination);
        zombie.directionChange = 30; // in this case the zombie probaly stuck somewhere
        setZombieSpeed(zombie);

        yield return new WaitForSeconds(timeBetweenSpawn);
        if(zombiesInGame.Count < zombiesNumber)
        {
            StartCoroutine(createZombies());
        }
    }
    //to make harder zombies, we set the walk animation speed of the zombies
    //they still think they walk, but actually they're running or ..
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
            zombie.speed_animationWalk = 0.8f;
        }
    }

    public bool checkMissionEnded()
    {
        bool noDeathButZombiesAlive = false;
        if(zombiesInGame.Count == zombiesNumber)
        {
            foreach(ZombieController enemies in zombiesInGame)
            {
                if (!enemies.isDead())
                {
                    noDeathButZombiesAlive = true;
                }
                if (Vector3.Distance(enemies.transform.position, destination.position) < 0.2f)
                {
                    //Oh no, the zombies reached the house, u will die.. :'(
                    PlayerController player = transform.root.gameObject.GetComponent<PlayerController>();
                    player.TakeDamage(player.currentHealth);
                    FailedMessage.SetActive(true);
                    failed = true;
                    return true;
                }
            }
            if (noDeathButZombiesAlive)
            {
                return false;
            }
            Debug.Log("Oke, u killed " + zombiesInGame.Count + " zombies, the mission was completed");
            CompletedMessage.SetActive(true);
            StartCoroutine(delayedTestCall(acceptWinning));
            return true;
        }
        return false;
    }

    public override void Go()
    {
        Wait = false;
    }

    public void Ended()
    {
        if (!failed)
        {
            Wait = false;
            chest.missionCompleted = true;

            //important!!!
            //this activates the  zombie creation
            //you should not start it earlier, otherwise the player would have no change
            factory.Active = true;
        }
        ended = true;
        StartCoroutine(weapon.OnUnScoped());
        weaponHolder.SetActive(false);
        dummyWeapon.SetActive(true);
        transform.root.gameObject.GetComponent<PlayerController>().weapon = null;

    }
}

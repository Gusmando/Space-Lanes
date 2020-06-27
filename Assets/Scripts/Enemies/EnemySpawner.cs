using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject shooterEnemy;
    public GameObject minorEnemy;
    public GameObject lobEnemy;
    public GameObject healthPrefab;
    public GameObject lane3assaultPrefab;
    public GameManager gameManager;
    public MovementController player;
    public int shooterEnemies;
    public int minorEnemies;
    public int lobEnemies;
    public int healthCount;
    public int X3LaneCount;
    public int waves;
    public float waveDelayTime;
    public float randMinMinor;
    public float randMaxMinor;
    public float randMinShoot;
    public float randMaxShoot;
    public float randMinLob;
    public float randMaxLob;
    public float randMinHealth;
    public float randMaxHealth;
    public float randMin3Lane;
    public float randMax3Lane;
    public float range;
    public float distanceToPlayer;
    public bool canSpawnShoot;
    public bool canSpawnMinor;
    public bool canSpawnLob;
    public bool canSpawnHealth;
    public bool canSpawn3Lane;
    public bool canWave;
    public int totalShooter;
    public int totalMinor;
    public int totalLob;
    public int totalHealth;
    public int total3Lane;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        canWave = true;
        totalShooter = shooterEnemies;
        totalMinor = minorEnemies;
        totalLob = lobEnemies;
        totalHealth = healthCount;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.subject.transform.position,this.transform.position);

        if(distanceToPlayer <= range && isActive)
        {
            isActive = false;
        }
        if(isActive)
        {
            if(canWave)
            {

                if(canSpawnMinor && minorEnemies!= 0)
                {
                    Instantiate(minorEnemy,transform.position ,transform.rotation);
                    minorEnemies--;
                    StartCoroutine(minorDelay(Random.Range(randMinMinor,randMaxMinor)));    
                }

                if(canSpawnShoot && shooterEnemies !=0)
                {
                    Instantiate(shooterEnemy,transform.position ,transform.rotation);
                    shooterEnemies --;
                    StartCoroutine(shooterDelay(Random.Range(randMinShoot,randMaxShoot))); 
                }

                if(canSpawnHealth && healthCount!=0)
                {
                    Instantiate(healthPrefab,transform.position ,transform.rotation);
                    healthCount --;
                    StartCoroutine(healthDelay(Random.Range(randMinHealth,randMaxHealth))); 
                }

                if(canSpawn3Lane && X3LaneCount!=0)
                {
                    Instantiate(lane3assaultPrefab,transform.position ,transform.rotation);
                    X3LaneCount --;
                    StartCoroutine(lane3Delay(Random.Range(randMin3Lane,randMax3Lane))); 
                }

                if(canSpawnLob && lobEnemies !=0 && gameManager.totalLob < gameManager.currentLanes.Length)
                {
                    Instantiate(lobEnemy,transform.position ,transform.rotation);
                    lobEnemies --;
                    StartCoroutine(lobDelay(Random.Range(randMinLob,randMaxLob))); 
                }

            }
            
            if(canWave && waves>= 0 && minorEnemies == 0 && shooterEnemies == 0 && lobEnemies == 0 && waves > 0)
            {
                shooterEnemies = totalShooter;
                minorEnemies = totalMinor;
                lobEnemies = totalLob;
                healthCount = totalHealth;
                X3LaneCount = total3Lane;
                waves--;
                canWave = false;
                StartCoroutine(waveDelay(waveDelayTime));
            }
        }

        
    }
       
    protected IEnumerator shooterDelay(float delayLength)
    {
        canSpawnShoot = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnShoot = true;

        yield return null;
    }
    protected IEnumerator minorDelay(float delayLength)
    {
        canSpawnMinor = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnMinor = true;

        yield return null;
    }
    protected IEnumerator waveDelay(float delayLength)
    {
        canWave = false;

        yield return new WaitForSeconds(delayLength);

        canWave = true;

        yield return null;
    }
    protected IEnumerator lobDelay(float delayLength)
    {
        canSpawnLob = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnLob = true;

        yield return null;
    }
    protected IEnumerator healthDelay(float delayLength)
    {
        canSpawnHealth = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnHealth = true;

        yield return null;
    }
    protected IEnumerator lane3Delay(float delayLength)
    {
        canSpawn3Lane = false;

        yield return new WaitForSeconds(delayLength);

        canSpawn3Lane= true;

        yield return null;
    }

}

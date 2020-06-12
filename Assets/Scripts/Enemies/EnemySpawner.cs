using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject shooterEnemy;
    public GameObject minorEnemy;
    public GameObject lobEnemy;
    public int shooterEnemies;
    public int minorEnemies;
    public int lobEnemies;
    public int waves;
    public float waveDelayTime;
    public float randMinMinor;
    public float randMaxMinor;
    public float randMinShoot;
    public float randMaxShoot;
    public float randMinLob;
    public float randMaxLob;  
    public bool canSpawnShoot;
    public bool canSpawnMinor;
    public bool canSpawnLob;
    public bool canWave;
    public int totalShooter;
    public int totalMinor;
    public int totalLob;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        canWave = true;
        totalShooter = shooterEnemies;
        totalMinor = minorEnemies;
        totalLob = lobEnemies;
    }

    // Update is called once per frame
    void Update()
    {
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

                if(canSpawnLob && lobEnemies !=0)
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

}

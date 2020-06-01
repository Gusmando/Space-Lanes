using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject shooterEnemy;
    public GameObject minorEnemy;
    public int shooterEnemies;
    public int minorEnemies;
    public float waveDelayTime;
    public float randMinMinor;
    public float randMaxMinor;
    public float randMinShoot;
    public float randMaxShoot; 
    int waves;
    public bool canSpawnShoot;
    public bool canSpawnMinor;
    public bool canWave;
    public int totalShooter;
    public int totalMinor;

    // Start is called before the first frame update
    void Start()
    {
        canWave = true;
        totalShooter = shooterEnemies;
        totalMinor = minorEnemies;

    }

    // Update is called once per frame
    void Update()
    {

        if(canWave)
        {
            if(canSpawnMinor)
            {
                minorEnemies--;
                StartCoroutine(minorDelay(Random.Range(randMinMinor,randMaxMinor)));    
            }

            if(canSpawnShoot)
            {
                shooterEnemies --;
                StartCoroutine(shooterDelay(Random.Range(randMinShoot,randMaxShoot))); 
            } 
        }

        if(minorEnemies == 0 && shooterEnemies == 0 && waves > 0 && !canWave)
        {
            StartCoroutine(waveDelay(waveDelayTime));
        }
        if(canWave && waves>= 0)
        {
            shooterEnemies = totalShooter;
            minorEnemies = totalMinor;
            waves--;
            canWave = false;
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

}

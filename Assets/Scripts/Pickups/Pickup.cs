using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public GameObject sprite;
    public float angleSpeed;
    public GameManager gameManager;
    public float minRand;
    public float maxRand;
    public abstract void action(GameObject Player);
    
    private void Start() 
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        Lane[] lanes;
        lanes = gameManager.currentLanes;
        int laneRand = Random.Range(0,lanes.Length);
        Vector3 lanePos = lanes[laneRand].position;
        Vector3 playerPos = GameObject.FindWithTag("Player").GetComponent<MovementController>().transform.position;
        float posMod = Random.Range(minRand,maxRand);
        this.transform.position = new Vector3(lanePos.x,lanePos.y,playerPos.z + posMod);
        this.transform.rotation = lanes[laneRand].rotation;
    }
    private void Update() 
    {
        sprite.transform.RotateAround(sprite.transform.position,sprite.transform.up,angleSpeed);    
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            action(other.gameObject);
            Destroy(gameObject);
        }
    }
}

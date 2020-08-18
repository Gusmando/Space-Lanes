using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [Header("Sprite")]
    public GameObject spriteBack;
    public GameObject sprite;
    public GameManager gameManager;

    [Header("Speed Scalars")]
    public float angleSpeed;
    public float minRand;
    public float maxRand;

    //Abstract action will be overwritten by each pickup
    public abstract void action(GameObject Player);
    
    private void Start() 
    {
        //Asssigning necessary references
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        //Lane assignment is randomized
        Lane[] lanes;
        lanes = gameManager.currentLanes;
        int laneRand = Random.Range(0,lanes.Length);
        
        //Spawn position based on player's position plus randomized value
        Vector3 lanePos = lanes[laneRand].position;
        Vector3 playerPos = GameObject.FindWithTag("Player").GetComponent<MovementController>().transform.position;
        float posMod = Random.Range(minRand,maxRand);
        this.transform.position = new Vector3(lanePos.x,lanePos.y,playerPos.z + posMod);
        this.transform.rotation = lanes[laneRand].rotation;
    }
    private void Update() 
    {
        //Constantly roatating the sprite
        sprite.transform.RotateAround(sprite.transform.position,sprite.transform.up,angleSpeed);
        spriteBack.transform.RotateAround(spriteBack.transform.position,spriteBack.transform.up,angleSpeed);
    }

    //On collision with player, action is called
    //and the pickup is destroyed
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            action(other.gameObject);
            Destroy(gameObject);
        }
    }
}

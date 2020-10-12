using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject[] enemies;
	public int enemiesSpawnedPerSpawner;
	public bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < enemiesSpawnedPerSpawner; i++){
        	if(canSpawn){
        		Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position , Quaternion.identity);
        	}        	
        	canSpawn = false;
        }
    }
}

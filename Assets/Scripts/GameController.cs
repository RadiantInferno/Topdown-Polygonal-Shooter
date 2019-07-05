using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Time inbetween each enemy spawn
    public float enemySpawnDelay;
    //The limits for where the enemy can spawn, where the x and y should be set to a value off the top right corner of the screen
    public Vector2 enemySpawnPos;

    //Variable to store the enemy prefab
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Begins the coroutine that spawns enemies regularly
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This is the coroutine that spawns the enemies
    IEnumerator SpawnEnemies()
    {
        //loops code infinitely
        while (true)
        {
            //wait "enemySpawnDelay" in seconds, then continue with code
            yield return new WaitForSeconds(enemySpawnDelay);
            //a variable to store where the next enemy will spawn
            Vector2 spawnPos;
            //Random choice that decides if the math used will be for the top/bottom or left/right side of the screen
            if (Random.value < 0.5)
            {
                spawnPos.x = Random.Range(-enemySpawnPos.x, enemySpawnPos.x);
                if (Random.value < 0.5) spawnPos.y = -enemySpawnPos.y;
                else spawnPos.y = enemySpawnPos.y;
            }
            else
            {
                spawnPos.y = Random.Range(-enemySpawnPos.y, enemySpawnPos.y);
                if (Random.value < 0.5) spawnPos.x = -enemySpawnPos.x;
                else spawnPos.x = enemySpawnPos.x;
            }
            //spawns an enemy at the randomised position
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
        
    }

}

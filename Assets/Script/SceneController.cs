using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
   [SerializeField]
   private GameObject enemyPrefab;
   private GameObject enemy;
   private Vector3 spawnPoint = new Vector3 (0, 0, 5);
   private int numberOfEnemies = 5;
   private GameObject[] enemies;

   void Start()
   {
    enemies = new GameObject[numberOfEnemies];
    for (int i = 0; i < numberOfEnemies; i++){
        SpawnEnemy(i);
    }

   }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++){
            if (enemies[i] ==null){
                 SpawnEnemy(i);
            }
        }
    }

    private void SpawnEnemy(int index){
        enemies[index] = Instantiate(enemyPrefab) as GameObject;

        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10) );
        enemies[index].transform.position = spawnPoint + randomOffset;

        float angle = Random.Range(0, 360);
        enemies[index].transform.Rotate(0, angle, 0);
    }
}

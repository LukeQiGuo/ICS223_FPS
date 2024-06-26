using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject enemy;
    private Vector3 spawnPoint = new Vector3(0, 0, 5);
    private int numberOfEnemies = 5;
    private GameObject[] enemies;

    [SerializeField] private GameObject iguanaPrefab;
    [SerializeField] private Transform iguanaSpawnPt;

    private GameObject[] iguanas;
    private int iguanaCount = 10;


    void Start()
    {
        enemies = new GameObject[numberOfEnemies];
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy(i);
        }

        iguanas = new GameObject[iguanaCount];
        for (int i = 0; i < iguanaCount; i++)
        {
            Vector3 spawnPosition = iguanaSpawnPt.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            iguanas[i] = Instantiate(iguanaPrefab, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                SpawnEnemy(i);
            }
        }
    }

    private void SpawnEnemy(int index)
    {
        enemies[index] = Instantiate(enemyPrefab) as GameObject;

        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        enemies[index].transform.position = spawnPoint + randomOffset;

        float angle = Random.Range(0, 360);
        enemies[index].transform.Rotate(0, angle, 0);
    }
}

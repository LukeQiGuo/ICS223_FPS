using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour

{
    [SerializeField] private UIController ui;
    private int score = 0;
    private int lastClipScore = 0; // 记录上次增加弹夹时的分数


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

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<int>.AddListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.RESTART_GAME, OnRestartGame);

    }
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<int>.RemoveListener(GameEvent.DIFFICULTY_CHANGED, OnDifficultyChanged);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.RESTART_GAME, OnRestartGame);
    }

    void Start()
    {
        ui.UpdateScore(score);

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
                WanderingAI ai = enemies[i].GetComponent<WanderingAI>();
                ai.SetDifficulty(GetDifficulty());
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

    private void OnEnemyDead()
    {
        score++;
        ui.UpdateScore(score);
        if (score - lastClipScore >= 5)
        {
            lastClipScore = score;
            Messenger<int>.Broadcast(GameEvent.CLIPS_CHANGED, 1);
        }
    }
    private void OnDifficultyChanged(int newDifficulty)
    {
        Debug.Log("Scene.OnDifficultyChanged(" + newDifficulty + ")");
        for (int i = 0; i < enemies.Length; i++)
        {
            WanderingAI ai = enemies[i].GetComponent<WanderingAI>();
            ai.SetDifficulty(newDifficulty);
        }
    }
    public int GetDifficulty()
    {
        return PlayerPrefs.GetInt("difficulty", 1);
    }

    private void OnPlayerDead()
    {
        ui.ShowGameOverPopup();
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }

}

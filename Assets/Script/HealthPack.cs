using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public ParticleSystem spawnEffect; // 引用Particle System

    void Start()
    {
        // 确保在游戏开始时不会自动播放特效
        if (spawnEffect != null)
        {
            spawnEffect.Stop();
        }
    }

    // public void OnSpawn()
    // {
    //     // 播放生成特效
    //     if (spawnEffect != null)
    //     {
    //         spawnEffect.Play();
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.TASK_COMPLETED); // 广播任务完成事件
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.TASK_COMPLETED); // 广播任务完成事件
            Destroy(gameObject);
        }
    }
}

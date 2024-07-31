using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public ParticleSystem spawnEffect;

    void Start()
    {

        if (spawnEffect != null)
        {
            spawnEffect.Stop();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.TASK_COMPLETED);
            Destroy(gameObject);
        }
    }
}

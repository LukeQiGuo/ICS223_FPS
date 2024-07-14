using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCharacter : MonoBehaviour
{
    public float maxHealth = 5f;
    private float health;

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PICKUP_HEALTH, OnPickupHealth);
    }


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void Hit()
    {
        health -= 1;
        Debug.Log("Health: " + health);

        float healthPercentage = health / maxHealth;
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, healthPercentage);

        if (health <= 0)
        {
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }

    public void OnPickupHealth(int healthAdded)
    {
        health += healthAdded;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        float healthPercent = (float)health / maxHealth;
        Messenger<float>.Broadcast(GameEvent.HEALTH_CHANGED, healthPercent);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCharacter : MonoBehaviour
{
    public float maxHealth = 5f;
    private float health;
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
            Debug.Break();
        }
    }

}

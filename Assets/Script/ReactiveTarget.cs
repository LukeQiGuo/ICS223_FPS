using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ReactiveTarget : MonoBehaviour
{
   [SerializeField] private GameObject healthPackPrefab;
   private bool isAlive = true;
   public void ReactToHit()
   {
      if (!isAlive) return;
      isAlive = false;

      Animator enemyAnimator = GetComponent<Animator>();
      if (enemyAnimator != null)
      {
         enemyAnimator.SetTrigger("Die");
      }

      WanderingAI enemyAI = GetComponent<WanderingAI>();
      if (enemyAI != null)
      {
         enemyAI.ChangeState(EnemyStates.dead);
      }

      Messenger.Broadcast(GameEvent.ENEMY_DEAD);


   }



   private void DeadEvent()
   {

      if (Random.value <= 0.2f)
      {
         Vector3 spawnPosition = transform.position;
         spawnPosition.y = 1.0f;
         Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);

      }
      Destroy(this.gameObject);
   }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ReactiveTarget : MonoBehaviour
{
   [SerializeField] private GameObject healthPackPrefab; // 新增
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

      // StartCoroutine(Die());
   }

   // private IEnumerator Die()
   // {
   //    // iTween.RotateAdd(this.gameObject, new Vector3(-75, 0, 0), 1);

   //    yield return new WaitForSeconds(2);

   //    Destroy(this.gameObject);
   // }

   private void DeadEvent()
   {
      // 10%的概率掉落急救箱
      if (Random.value <= 0.9f)
      {
         Vector3 spawnPosition = transform.position;
         spawnPosition.y = 1.0f; // 设置生成位置的 y 轴值为 1.0
         Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);

      }
      Destroy(this.gameObject);
   }

}

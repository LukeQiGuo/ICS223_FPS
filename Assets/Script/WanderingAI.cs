using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { alive, dead }

public class WanderingAI : MonoBehaviour
{
    private EnemyStates state;
    [SerializeField] private GameObject laserbeamPrefab;
    private GameObject laserbeam;
    private float fireRate = 2.0f;
    private float nextFire = 0.0f;
    private float enemySpeed = 1.5f;
    private float baseSpeed = 0.25f;
    private float difficultySpeedDelta = 0.3f;
    private float obstacleRange = 5.0f;
    private float sphereRadius = 0.75f;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private float chaseRange = 10.0f;
    [SerializeField] private float chaseSpeed = 2.5f;

    [SerializeField] private float meleeAttackRange = 1.5f;
    [SerializeField] private float meleeAttackCooldown = 8.0f;
    private float lastMeleeAttackTime = 0.0f;
    private Animator animator;
    [SerializeField] private Transform[] waypoints; // 路径点数组
    private int currentWaypointIndex = 0; // 当前路径点索引

    void Start()
    {
        state = EnemyStates.alive;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = baseSpeed;
        playerTransform = GameObject.FindWithTag("Player").transform;

        animator = GetComponent<Animator>();


        // 查找所有带有 "Waypoint" 标签的对象，并填充 waypoints 数组
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        // 随机打乱巡逻点的顺序
        ShuffleWaypoints();

        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyStates.alive)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer < chaseRange)
            {

                navMeshAgent.speed = chaseSpeed;
                navMeshAgent.SetDestination(playerTransform.position);

                if (distanceToPlayer < meleeAttackRange)
                {

                    animator.SetBool("IsAttacking", true);
                    PerformMeleeAttack();
                }
                else
                {

                    animator.SetBool("IsAttacking", false);
                    ShootAtPlayer();
                }
            }

            // else
            // {
            //     Vector3 movement = Vector3.forward * enemySpeed * Time.deltaTime;
            //     transform.Translate(movement);

            //     Ray ray = new Ray(transform.position, transform.forward);
            //     RaycastHit hit;
            //     if (Physics.SphereCast(ray, sphereRadius, out hit))
            //     {
            //         GameObject hitObject = hit.transform.gameObject;
            //         if (hitObject.GetComponent<PlayCharacter>())
            //         {
            //             ShootAtPlayer();
            //             // if (laserbeam == null && Time.time > nextFire)
            //             // {
            //             //     nextFire = Time.time + fireRate;
            //             //     laserbeam = Instantiate(laserbeamPrefab) as GameObject;
            //             //     laserbeam.transform.position = transform.TransformPoint(0, 1.5f, 1.5f);
            //             //     laserbeam.transform.rotation = transform.rotation;
            //             // }
            //         }
            //         else if (hit.distance < obstacleRange)
            //         {
            //             float turnAngle = Random.Range(-110, 110);
            //             transform.Rotate(Vector3.up * turnAngle);
            //         }
            //     }
            // }
            else
            {
                Patrol();
            }

        }
    }

    private void Patrol()
    {
        navMeshAgent.speed = enemySpeed;

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 17) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }


    private void PerformMeleeAttack()
    {
        if (Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            lastMeleeAttackTime = Time.time;
            OnAttackHit();
        }
    }

    private void OnAttackHit()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < meleeAttackRange)
        {
            PlayCharacter player = playerTransform.GetComponent<PlayCharacter>();
            if (player != null)
            {
                player.Hit();
            }
        }
    }
    private void ShootAtPlayer()
    {
        if (laserbeam == null && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            laserbeam = Instantiate(laserbeamPrefab) as GameObject;
            laserbeam.transform.position = transform.TransformPoint(0, 1.5f, 1.5f);
            laserbeam.transform.rotation = transform.rotation;
        }
    }
    public void ChangeState(EnemyStates state)
    {
        this.state = state;
        if (state == EnemyStates.dead)
        {
            animator.SetBool("IsAttacking", false);
            GetComponent<NavMeshAgent>().enabled = false;
        }

    }


    private void ShuffleWaypoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform temp = waypoints[i];
            int randomIndex = Random.Range(i, waypoints.Length);
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 rangeTest = transform.position + transform.forward * obstacleRange;
        Debug.DrawLine(transform.position, rangeTest);
        Gizmos.DrawWireSphere(rangeTest, sphereRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
    public void SetDifficulty(int difficulty)
    {
        Debug.Log("WanderingAI.SetDifficulty(" + difficulty + ")");
        enemySpeed = baseSpeed + (difficulty * difficultySpeedDelta);
    }
}

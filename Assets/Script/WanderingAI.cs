using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { alive, dead }

public class WanderingAI : MonoBehaviour
{
    private EnemyStates state;
    [SerializeField]
    private GameObject laserbeamPrefab;
    private GameObject laserbeam;
    private float fireRate = 2.0f;
    private float nextFire = 0.0f;
    private float enemySpeed = 3.0f;
    private float obstacleRange = 5.0f;
    private float sphereRadius = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.alive;
    }

    // Update is called once per frame
    void Update()
    { 
        if(state == EnemyStates.alive){
        Vector3 movement = Vector3.forward * enemySpeed *Time.deltaTime;
        transform.Translate(movement);

        Ray ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast (ray, sphereRadius, out hit)){
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<PlayCharacter>())
            {
                if (laserbeam == null && Time.time > nextFire){
                    nextFire = Time.time + fireRate;
                    laserbeam = Instantiate (laserbeamPrefab) as GameObject;
                    laserbeam.transform.position = transform.TransformPoint (0, 1.5f, 1.5f);
                    laserbeam.transform.rotation = transform.rotation;
                }
            }
            else if (hit.distance < obstacleRange){
                float turnAngle = Random.Range (-110, 110);
                transform.Rotate (Vector3.up * turnAngle);
                }
            }
        }
    }

    public void ChangeState(EnemyStates state){
        this.state = state;
    }

    private void  OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 rangeTest = transform.position + transform.forward * obstacleRange;
        Debug.DrawLine(transform.position, rangeTest);
        Gizmos.DrawWireSphere(rangeTest, sphereRadius);
    }
}

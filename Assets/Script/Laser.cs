using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 6f;
    [SerializeField]
    private Rigidbody rb;
void FixedUpdate()
{
    float toNewtons = 100;
    Vector3 movement = transform.forward * Time.deltaTime * speed * toNewtons;
    rb.velocity = movement;
}
  
  void OnTriggerEnter(Collider other)
  {
   PlayCharacter player = other.GetComponent<PlayCharacter> ();
   if (player != null) {
    player.Hit ();
   }
   Destroy (this.gameObject);
  }
}

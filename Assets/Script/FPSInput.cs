using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
    private float speed = 9.0f;
    

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput =Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput) * Time.deltaTime * speed ;
        transform.Translate(movement);
        
    }
}

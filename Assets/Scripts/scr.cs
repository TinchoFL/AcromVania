using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   [SerializeField] float moveSpeed = 1f;
   Rigidbody2D myRigidbody;


    void Start()
    {
        
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
    }
}

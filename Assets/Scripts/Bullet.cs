using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    [SerializeField] AudioClip bulletSFX;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        AudioSource.PlayClipAtPoint(bulletSFX, Camera.main.transform.position);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
        //FlipBulletSprite();
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position);
        }
        Destroy(gameObject);
    }

    /*    void FlipBulletSprite(){

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed){
             transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        }    
        
    }
    */

    void OnCollisionEnter2D(Collision2D other){
        Destroy(gameObject, 0.3f);
    }
}

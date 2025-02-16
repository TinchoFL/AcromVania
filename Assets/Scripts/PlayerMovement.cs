using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float arrowLooseDelay = 0.2f;
    [SerializeField] AudioClip playerDeathSFX;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

   
    void Update()
    {
        if(!isAlive){
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    /*
    void OnFire(InputValue value){
          if(!isAlive){
            return;
        }
        Instantiate(bullet, gun.position, transform.rotation);

    }
    */

    
    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            myAnimator.SetTrigger("IsShooting");
            Invoke("ArrowLoose", arrowLooseDelay);
        }
    }

    void ArrowLoose()
    {
        if (myRigidbody.transform.localScale.x < Mathf.Epsilon) // <-- Player facing left
        {
            Instantiate(bullet, gun.position, transform.rotation);        }
        if (myRigidbody.transform.localScale.x > Mathf.Epsilon) // <-- Player facing right
        {
            
            Instantiate(bullet, gun.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }
    }



    void OnMove(InputValue value){

        if(!isAlive){
            return;
        }
        moveInput = value.Get<Vector2>();
        
    }

   void OnJump(InputValue value)
    {
        if(!isAlive){
            return;
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}
        bool isTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isTouchingLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!isTouchingGround && !isTouchingLadder) { return;}
        
        if(value.isPressed)
        {
            // do stuff
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }



    void Run(){
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }
    
    void FlipSprite(){

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed){
             transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

     void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("IsClimbing", false);
            return;
        }
        
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }

    void Die(){
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))){
            
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            AudioSource.PlayClipAtPoint(playerDeathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

}


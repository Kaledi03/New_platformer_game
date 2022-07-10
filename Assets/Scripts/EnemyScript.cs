using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    private Transform transform;
    private bool isFacingRight = true;
    public Animator animator;
    public double minimumNoticingDistance;
    private bool isNoticeAnimationDone = false;
    public LayerMask whatIsGround;
    public Transform groundFallCheck;
    public double walkSpeed;
    public Rigidbody2D rb;
    public double checkRadius;
    bool isMoving;
    public PlayerMovment playerScript;
    public float minXrange;
    public float minYrange;
    private bool isNotInRange;
    float remainingLives = 2;
    private bool dead;
    public Collider2D collider;
    private bool isCalled;
    public float waitTime;
    public LayerMask whatIsPlayer;
    public Transform centralPoint;
    private bool GroundCheck;
    public Transform wallSensor;
    public float rechargeTime;
    public float damageRange;


    void Start(){
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        playerScript = FindObjectOfType<PlayerMovment>();
    }


    void Update(){
        if(!dead){
            isNotInRange = Mathf.Abs(transform.position.x - player.transform.position.x) > minXrange;
            if(player.transform.position.x > transform.position.x && isNotInRange){
                if(!isFacingRight){
                    Flip();
                }
            }else if(transform.position.x > player.transform.position.x && isNotInRange){
                if(isFacingRight){
                    Flip();
                }
            }
        } 
    }


    void FixedUpdate(){
        if(!dead){

            GroundCheck = Physics2D.OverlapCircle(groundFallCheck.position, (float)checkRadius, whatIsGround);
            bool nearWall = Physics2D.OverlapCircle(wallSensor.position, (float)checkRadius, whatIsGround);

            if(GroundCheck && !nearWall){
                if (isFacingRight && (player.transform.position.x - transform.position.x > minXrange))
                {
                    rb.velocity = new Vector2((float)walkSpeed, rb.velocity.y);
                    isMoving = true; 
                }else if(!isFacingRight && (transform.position.x - player.transform.position.x > minXrange)){
                    rb.velocity = new Vector2(-(float)walkSpeed, rb.velocity.y);
                    isMoving = true;
                }else{
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    isMoving = false;  
                }
            }else{
                rb.velocity = new Vector2(0, rb.velocity.y);
                isMoving = false;
            }

            if((Physics2D.OverlapCircle(centralPoint.position, 0.7f, whatIsPlayer)) && !isCalled){
                StartCoroutine("kill");
                isCalled = true;
                
            }

            animator.SetBool("isMoving", isMoving);

            if((isFacingRight) && ((player.transform.position.x - transform.position.x) < minimumNoticingDistance) && (!isNoticeAnimationDone)){
                animator.SetTrigger("Noticed");
                isNoticeAnimationDone = true;
            }else if((!isFacingRight) && ((transform.position.x - player.transform.position.x) < minimumNoticingDistance) && (!isNoticeAnimationDone)){
                animator.SetTrigger("Noticed");
                isNoticeAnimationDone = true;
            }
            if((isFacingRight) && ((player.transform.position.x - transform.position.x) > minimumNoticingDistance) && (isNoticeAnimationDone)){
                isNoticeAnimationDone = false;
            }else if((!isFacingRight) && ((transform.position.x - player.transform.position.x) > minimumNoticingDistance) && (isNoticeAnimationDone)){
                isNoticeAnimationDone = false;
            }
        }else{
            animator.SetTrigger("Dead");
        }
    }

    public void Flip(){
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }


    public IEnumerator kill(){
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Killing");
        isCalled = false;
    }

    public IEnumerator isCalledTrue(){
        yield return new WaitForSeconds(rechargeTime);
        isCalled = false;
    }

    void OnMouseDown () {
        if (Input.GetMouseButtonDown(0)) {
            if(PlayerInRange())
            {
                Debug.Log("hit");
                if(remainingLives >= 1){
                animator.SetTrigger("isAttacked");
                remainingLives --;
                }else{
                    animator.SetTrigger("Dead");
                    dead = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                    collider.isTrigger = true;
                } 
            }
        }
    }

    bool PlayerInRange(){
        if(((playerScript.getFacingRight()) && (transform.position.x-player.transform.position.x <= damageRange)) ||
            ((!playerScript.getFacingRight()) && (player.transform.position.x-transform.position.x <= damageRange))){
            return true;
        }
        return false;
    }
}

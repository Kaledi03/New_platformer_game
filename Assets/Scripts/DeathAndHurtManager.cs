using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndHurtManager : MonoBehaviour
{
    public Transform transform;
    public Vector3 CheckPoint;
    public SpriteRenderer sprite_renderer;
    public float After_Death_Time;
    public Transform cameraTransform;
    public PlayerMovment movmentScript;
    public float waiting_time; 
    public CameraShake shakeScript;
    private Vector3 cameraChckpoint;
    public Animator transiotionAnim;
    private bool called = false;
    public ParticleSystem particleEffect;
    private Vector3 startPoint;
    int remainingLives = 2;
    public Animator heartImm;
    
    void Start()
    {
        transform = GetComponent<Transform>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        movmentScript = GetComponent<PlayerMovment>();
        startPoint = transform.position;
        CheckPoint = transform.position;
        cameraChckpoint = cameraTransform.position;
    }

    void Update(){
        
    }

    public void Hurt(){
        if(!called){
            shakeScript.callShake();
            StartCoroutine("Transition_Closer");
            transiotionAnim.SetTrigger("end");
            called = true;
            sprite_renderer.color = new Color(255f, 255f, 255f, 0f);
            particleEffect.Play();
            //particle effect
        }
    }
    
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Obstacle"){
            Hurt();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "EnemyWeapon"){
            Hurt();
        }
    }

    public void NormalDeath(){
        StartCoroutine("transitionOpener");
        transiotionAnim.SetTrigger("end");
    }

    public IEnumerator normalDeathWait(){
        yield return new WaitForSeconds(0.4f);
        StartCoroutine("Transition_Closer");
        transiotionAnim.SetTrigger("end");
    }

    public IEnumerator transitionOpener(){
        yield return new WaitForSeconds(waiting_time);
        if(remainingLives > 0){
            transform.position = CheckPoint;
            transiotionAnim.SetTrigger("start");
            remainingLives --;
            if(remainingLives == 1){
                heartImm.SetTrigger("Half");
            }else{
                heartImm.SetTrigger("Empty");
            }
        }else{
            transform.position = startPoint;
            
            transiotionAnim.SetTrigger("start");
            remainingLives = 2;
            heartImm.SetTrigger("Normal");
        }
    }
    
    public IEnumerator Transition_Closer(){
        yield return new WaitForSeconds(waiting_time);
        if(remainingLives > 0){
            transform.position = CheckPoint;
            sprite_renderer.color = new Color(255f, 255f, 255f, 1f);
            transiotionAnim.SetTrigger("start");
            called = false; 
            remainingLives --;
            if(remainingLives == 1){
                heartImm.SetTrigger("Half");
            }else{
                heartImm.SetTrigger("Empty");
            }
        }else{
            transform.position = startPoint;
            sprite_renderer.color = new Color(255f, 255f, 255f, 1f);
            transiotionAnim.SetTrigger("start");
            called = false;
            StartCoroutine("setRemainingLives2");
            heartImm.SetTrigger("Normal");
        }
        
    }

    public IEnumerator setRemainingLives2(){
        yield return new WaitForSeconds(0.4f);
        remainingLives = 2;
    }
   

    
}

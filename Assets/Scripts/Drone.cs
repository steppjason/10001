using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{

    [SerializeField] float speed = 0.1f;
    [SerializeField] float rotSpeed = 3f;
    [SerializeField] float aggroRange = 50f;
    [SerializeField] float acceleration = 1f;

    [SerializeField] Animator animator;


    private const float DRONE_ROTATION = 270f;
    
    private float velocity = 0f;

    private GameObject player;
    private Vector3 target;

    private float maxSpeed = 12f;

    private float life = 1f;

    

    Coroutine thrustCoroutine;
    Coroutine death;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.position;

        animator.SetInteger("Dead", 0);
    
    }

    // Update is called once per frame
    void Update()
    {

         Move();
    }

    public void Move(){

        if(player.activeInHierarchy){

            thrustCoroutine = StartCoroutine(ChangeSpeed());

            velocity = Mathf.Clamp(velocity,0,maxSpeed);

            float distance = Vector3.Distance (player.transform.position, transform.position);
            
            if(distance < aggroRange){

                //Get the player's current position
                target = player.transform.position;

                //Use Slerp to rotate towards the player's position
                transform.rotation = Quaternion.Slerp(
                        transform.rotation, 
                        Quaternion.Euler(new Vector3(0, 0, 
                            Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg + DRONE_ROTATION)), 
                        rotSpeed * Time.deltaTime);

                transform.position += transform.rotation * Vector3.up * velocity * Time.deltaTime;

            }
        } else {
            velocity = 0;
            StopCoroutine(thrustCoroutine);
        }


        if(Input.GetButtonDown("Fire2")){
            Death();
        }

    }

    private void Death(){
        animator.SetInteger("Dead", 1);
    }

    private void Reset(){
        gameObject.SetActive(false);
        life = 1f;
    }


    IEnumerator ChangeSpeed(){
        while(true){
            velocity += speed;
            yield return new WaitForSeconds(acceleration);
        }
    }

}

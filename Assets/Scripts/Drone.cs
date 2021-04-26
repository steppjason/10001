using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{

    [SerializeField] float speed = 0.05f;
    [SerializeField] float rotSpeed = 5f;
    [SerializeField] float aggroRange = 10f;
    [SerializeField]private float maxSpeed = 12f;
    [SerializeField]private int damage = 1;

    [SerializeField] Animator animator;

    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource randomSound1;
    [SerializeField] AudioSource randomSound2;


    private const float DRONE_ROTATION = 270f;
    
    private float velocity = 0f;

    [SerializeField] Player pl;

    private GameObject player;
    private Vector3 target;

    private float life = 1f;
    private bool isDead = false;

    
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

        if(player.activeInHierarchy && !isDead && !pl.PlayerDead()){

            velocity += Time.deltaTime * speed;

            velocity = Mathf.Clamp(velocity,0,maxSpeed);

            float distance = Vector3.Distance (player.transform.position, transform.position);
            
            if(distance < aggroRange){

                
                target = player.transform.position;

                transform.rotation = Quaternion.Slerp(
                        transform.rotation, 
                        Quaternion.Euler(new Vector3(0, 0, 
                            Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg + DRONE_ROTATION)), 
                        rotSpeed * Time.deltaTime);

                transform.position += transform.rotation * Vector3.up * velocity * Time.deltaTime;

            }
        } else {
            velocity = 0;
        }

    }

    public void Damage(int damage){
        life -= damage;
        
        if(life <= 0)
            Death();
    }

    private void Death(){
        isDead = true;
        animator.SetInteger("Dead", 1);
        if(!deathSound.isPlaying)
            deathSound.Play();
    }

    private void Reset(){
        gameObject.SetActive(false);
        life = 1f;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.GetComponent<Player>()){
            if(!attackSound.isPlaying)
                attackSound.Play();
            other.gameObject.GetComponent<Player>().Damage(damage);
        }
    }


  

}

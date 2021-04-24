using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 100f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float thrustRate = 0.1f;
    [SerializeField] private float maxSpeed = 5f;

    private float velocity = 0.0f;
    private float direction;

    Coroutine thrustCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        direction = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("SpeedUp")){
            thrustCoroutine = StartCoroutine(ChangeSpeed(1));
        }

        if(Input.GetButtonUp("SpeedUp")){
            StopCoroutine(thrustCoroutine);
        }

        if(Input.GetButtonDown("SpeedDown")){
            thrustCoroutine = StartCoroutine(ChangeSpeed(0));
        }

        if(Input.GetButtonUp("SpeedDown")){
            StopCoroutine(thrustCoroutine);
        }

        
        velocity = Mathf.Clamp(velocity,0,maxSpeed);

        this.transform.Rotate(Vector3.forward * -1 * direction * rotSpeed * Time.deltaTime);
        this.transform.position += this.transform.rotation * Vector3.up * velocity * Time.deltaTime;
    }

    IEnumerator ChangeSpeed(float thrust){
        while(true){
            if(thrust == 1){
                velocity += speed;
            }

            if(thrust == 0){
                velocity -= speed;
            }

            yield return new WaitForSeconds(thrustRate);
        }
    }
}

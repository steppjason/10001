using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 100f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float thrustRate = 0.1f;
    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] GameObject gun;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] PlayerBullet[] bullets;
    [SerializeField] int numberOfBullets = 20;
    [SerializeField] GameObject parentBulletPool;

    [SerializeField] AudioClip fireSoundFX;
    [SerializeField] float fireSFXVolume;

    private PlayerBullet[] bulletList;
    private int nextBullet = 0;
    private Vector3 bulletDirection;

    private float velocity = 0.0f;
    private float direction;

    Coroutine thrustCoroutine;
    Coroutine fireCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        InstantiateBullets();
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

        Aim();


        if(Input.GetButtonDown("Fire1")){
            fireCoroutine = StartCoroutine(Firing());
        }

        if(Input.GetButtonUp("Fire1")){
            StopCoroutine(fireCoroutine);
        }

        
    }

    private void Aim(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - gun.transform.position + new Vector3(0,0,90));

        
    }

    public void Fire(Vector3 target){
        GetAvailable();
        bulletList[nextBullet].ResetLife();
        bulletList[nextBullet].transform.position = gun.transform.position;
        bulletList[nextBullet].transform.rotation = gun.transform.rotation;
        bulletList[nextBullet].SetDirection(target);
        bulletList[nextBullet].gameObject.SetActive(true);
        //audio.PlaySFX(clip);
    }

    private void InstantiateBullets(){
        bulletList = new PlayerBullet[numberOfBullets];
        for(int i = 0; i < numberOfBullets; i++){
            bulletList[i] = Instantiate(bullets[0], new Vector3(-25,0,0), Quaternion.identity);
            bulletList[i].transform.parent = parentBulletPool.transform;
            bulletList[i].gameObject.SetActive(false);            
        }
    }

    private void GetAvailable(){
        for(int i = 0; i < bulletList.Length; i++){
            if(!bulletList[i].gameObject.activeInHierarchy){
                nextBullet = i;
                return;
            }
        }
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

    IEnumerator Firing(){
        while(true){
            bulletDirection = Input.mousePosition;
            bulletDirection.z = 0.0f;
            bulletDirection = Camera.main.ScreenToWorldPoint(bulletDirection);
            bulletDirection.z = 0.0f;

            bulletDirection.x = bulletDirection.x - transform.position.x;
            bulletDirection.y = bulletDirection.y - transform.position.y;
            
            this.GetComponent<AudioSource>().Play();
            //AudioSource.PlayClipAtPoint(fireSoundFX, Camera.main.transform.position, fireSFXVolume);

            

            Fire(bulletDirection.normalized);
            yield return new WaitForSeconds(fireRate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    

    [Header("Weapon")]
    [Space(10)]
    [SerializeField] GameObject gun;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] PlayerBullet[] bullets;
    [SerializeField] int numberOfBullets = 20;
    [SerializeField] GameObject parentBulletPool;

    [SerializeField] private float coolDownRate = 0.1f;
    [SerializeField] private float heatUpRate = 0.1f;
    [SerializeField] private float MAX_DELAY = 0.22f;

    [SerializeField] AudioSource fireSoundFX;

    [SerializeField] Image weaponUIImage;
    [SerializeField] TextMeshProUGUI weaponTempText;

    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] AudioSource warningAudio;
    [SerializeField] private float WARNING_BEEP = 0.5f;

    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] AudioSource errorMessage;
    [SerializeField] private float MAX_ERROR_DELAY = 3f;
    private float errorTime = 0;

    private float warningTime = 0;

    private PlayerBullet[] bulletList;
    private int nextBullet = 0;
    private Vector3 bulletDirection;

    private float weaponTemp = 0f;
    private float waitDelay = 0;
    private bool overheated = false;

    
    

    [Space(10)]

    [Header("Thrusters")]

    [Space(10)]

    [SerializeField] private float rotSpeed = 100f;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float thrustRate = 0.002f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] Image thrusterUIImage;
    [SerializeField] TextMeshProUGUI thrustUIText;
    [SerializeField] GameObject thrusterSprite;
    [SerializeField] GameObject fullThrusterSprite;
    [SerializeField] AudioSource thrusterAudio;
    private bool thrustersAlreadyPlaying = false;


    
    [SerializeField] private float hullIntegrity = 100f;

    [SerializeField] TextMeshProUGUI hullText;
    [SerializeField] TextMeshProUGUI dangerText;
    [SerializeField] AudioSource dangerSound;
    [SerializeField] private float MAX_DANGER_DELAY = 3f;
    private float dangerTime = 0;


    [Header("Coordinates")]
    [SerializeField] TextMeshProUGUI coordValue;
    [SerializeField] float initialOffsetX = -3145f;
    [SerializeField] float initialOffsetY = 161f;
    [SerializeField] float multiplier = 2.7f;

    [SerializeField] Animator playerAnimation;


    private float velocity = 0.0f;
    private float direction;
    public bool dead = false;

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
        
        

        

        
        

        

        

        if(!dead){

            Aim();

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

            if(Input.GetButtonDown("Fire1")){
                fireCoroutine = StartCoroutine(Firing());
            }

            if(Input.GetButton("Fire1") && overheated){
                
                if(errorTime >= MAX_ERROR_DELAY){
                    errorMessage.Play();
                    errorTime = 0;
                }
                
            }

            direction = Input.GetAxisRaw("Horizontal");

        }

        velocity = Mathf.Clamp(velocity,0,maxSpeed);
        weaponTemp = Mathf.Clamp(weaponTemp,0,1);
        hullIntegrity = Mathf.Clamp(hullIntegrity,0,100);

        this.transform.Rotate(Vector3.forward * -1 * direction * rotSpeed * Time.deltaTime);
        this.transform.position += this.transform.rotation * Vector3.up * velocity * Time.deltaTime;


        if(Input.GetButtonUp("Fire1")){
            StopCoroutine(fireCoroutine);
        }
        

        if(!Input.GetButton("Fire1") || overheated)
            CoolDownWeapon();

        UpdateHullUI();
        UpdateThrusterUI();
        UpdateHeatTemp();
        ShowThrustSprite();
        UpdateCoords();
        
       

        if(overheated)
            errorText.color = Color.Lerp(errorText.color, new Color(1,0,0.2190539f,1), Time.deltaTime);
        else
            errorText.color = Color.Lerp(errorText.color, new Color(1,0,0.2190539f,0), Time.deltaTime * 2f);


        if(hullIntegrity < 35 && !dead){
            dangerText.color = Color.Lerp(new Color(1,0.4445944f,0,1), new Color(1,0.4445944f,0,0.3686275f), Mathf.PingPong(Time.time * 2.5f, 1.0f));

            if(dangerTime >= MAX_DANGER_DELAY){
                dangerSound.Play();
                dangerTime = 0;
            }
        }
            
        
        errorTime += Time.deltaTime;
        dangerTime += Time.deltaTime;
        
    }

    private void UpdateCoords(){
        coordValue.text = "[ " + ((this.transform.position.x + initialOffsetX) * multiplier).ToString("#.##")  + " , " + ((this.transform.position.y + initialOffsetY) * multiplier).ToString("#.##") + " ]";
    }

    private void ShowThrustSprite(){
        
        if(Input.GetButton("SpeedDown")){

            if(thrusterAudio.isPlaying)
                thrusterAudio.Stop();
            
            thrusterSprite.SetActive(false);
            fullThrusterSprite.SetActive(false);
            
        }
        else if( (velocity / maxSpeed) * 100 >= 50 ){

            if(!thrusterAudio.isPlaying)
                thrusterAudio.Play();
            
            fullThrusterSprite.SetActive(true);
            thrusterSprite.SetActive(false);
        } 
        else if( (velocity / maxSpeed) * 100  > 0 ){

            if(!thrusterAudio.isPlaying)
                thrusterAudio.Play();
            
            thrusterSprite.SetActive(true);
            fullThrusterSprite.SetActive(false);
        } else {
            
            if(thrusterAudio.isPlaying)
                thrusterAudio.Stop();
            
            thrusterSprite.SetActive(false);
            fullThrusterSprite.SetActive(false);
        }

       
        
        
    }

    public float GetPlayerSpeed(){
        return velocity;
    }

    public bool PlayerDead(){
        return dead;
    }

    private void UpdateThrusterUI(){
        thrusterUIImage.fillAmount = velocity / maxSpeed;
        thrustUIText.text = Mathf.Round((velocity / maxSpeed) * 100) + "%";
    }

    private void UpdateHullUI(){
        hullText.text = Mathf.Round(hullIntegrity).ToString() + " / 100";
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
        HeatUpWeapon();
    }


    public void UpdateHeatTemp(){
        weaponUIImage.fillAmount = weaponTemp;
        Color overheatedColor = new Color(1,0.1897309f,0);
        weaponUIImage.color = Color.Lerp(Color.white, overheatedColor,  weaponTemp);
        weaponTempText.text = Mathf.Round(weaponTemp * 275).ToString() + "°C";

        if(weaponTemp > 0.7f && !overheated){

            if(warningTime >= WARNING_BEEP){
                warningAudio.Play();
                warningTime = 0;
            }
            
            warningText.color = Color.Lerp(new Color(1,0.4445944f,0,1), new Color(1,0.4445944f,0,0.3686275f), Mathf.PingPong(Time.time * 1.5f, 1.0f));
        } else {
            warningText.color = Color.Lerp(warningText.color, new Color(1,0.4445944f,0,0), Time.deltaTime * 2f);
        }

        warningTime += Time.deltaTime;

            
    }

    public void HeatUpWeapon(){
        weaponTemp += heatUpRate;

        if(weaponTemp >= 1){
            overheated = true;
            waitDelay = MAX_DELAY;
        }

    }

    public void CoolDownWeapon(){
        

        if(overheated && waitDelay > 0){
            waitDelay -= 1 * Time.deltaTime;
        } 
        else if(overheated && weaponTemp <= 0) {
                overheated = false;
                
        } 
        else {
            weaponTemp -= coolDownRate * Time.deltaTime;
        }
    }

    public void Damage(int damage){
        hullIntegrity -= damage;
        if(hullIntegrity <= 0)
            Death();
    }

    public void Death(){
        dead = true;
        velocity = 0;
        playerAnimation.SetBool("Dead",true);
        gun.SetActive(false);
        
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
            if(!overheated){
                bulletDirection = Input.mousePosition;
                bulletDirection.z = 0.0f;
                bulletDirection = Camera.main.ScreenToWorldPoint(bulletDirection);
                bulletDirection.z = 0.0f;

                bulletDirection.x = bulletDirection.x - transform.position.x;
                bulletDirection.y = bulletDirection.y - transform.position.y;
                

                fireSoundFX.Play();

                Fire(bulletDirection.normalized);
            } 
            yield return new WaitForSeconds(fireRate);
        }
    }
}

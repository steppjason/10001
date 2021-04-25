using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed = 0f;
    [SerializeField] Vector3 direction = new Vector3(0,0,0);
    [SerializeField] int damage = 1;
    [SerializeField] float MAX_LIFE = 0f;

    private float life = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy && speed > 0){
            transform.position += direction * speed * Time.deltaTime;
        }

        life += Time.deltaTime;
        if(life > MAX_LIFE)
            gameObject.SetActive(false);

    }

    public void ResetLife(){
        life = 0;
    }

    public void SetDirection(Vector3 direction){
        this.direction = direction;
    }

    public void SetSpeed(float speed){
        this.speed = speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        gameObject.SetActive(false);

        if(other.gameObject.GetComponent<Drone>()){
            other.gameObject.GetComponent<Drone>().Damage(damage);
        }
    }
}

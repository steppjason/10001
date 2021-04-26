using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Player pl;
    [SerializeField] float distance = 1f;

    float playerSpeed;

    Material _material;
    float offsetX;
    float offsetY;
    Vector3 playerDirection;

    // Start is called before the first frame \update
    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;

        playerSpeed = pl.GetPlayerSpeed();
        playerDirection = player.transform.up;
        
        

        offsetX += (playerDirection.x * playerSpeed) / distance * Time.deltaTime;
        offsetY += (playerDirection.y * playerSpeed) / distance * Time.deltaTime;

        _material.mainTextureOffset = new Vector2(offsetX, offsetY);


        
    }
}

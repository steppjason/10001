using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Camera m_MainCamera;
    
    [SerializeField] float moveSpeed = 1f;
    private Vector3 direction;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);        
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    private Camera m_MainCamera;
    
    [SerializeField] float moveSpeed = 1f;
    private Vector3 direction;

    private bool isMenu = true;

    [SerializeField] RawImage blackBG;
    [SerializeField] Image menuBG;
    [SerializeField] TextMeshProUGUI title;

    [SerializeField] GameObject game;

    private float fadetime = 1.5f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);        
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;*/

        

        if(isMenu){
            if(Input.GetButton("Jump")){
          
                
                isMenu = false;
                game.SetActive(true);

            }

        }

        if(!isMenu){
                blackBG.color = Color.Lerp(blackBG.color, new Color(0,0,0,0), Time.deltaTime * fadetime);
                menuBG.color = Color.Lerp(menuBG.color, new Color(0,0,0,0), Time.deltaTime * fadetime);
                title.color = Color.Lerp(title.color, new Color(0,0,0,0), Time.deltaTime * fadetime);
            }
    }
}

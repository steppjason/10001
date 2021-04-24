using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField] Animator animator;
    private float direction;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("Direction", 0);
        animator.SetInteger("Reverse", 0);
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        animator.SetInteger("Direction", (int)direction);
        
        if(Input.GetButton("SpeedDown")){
            animator.SetInteger("Reverse", 1);
        }
        else
            animator.SetInteger("Reverse", 0);

        Debug.Log(Input.GetButtonDown("SpeedDown"));
    }
}

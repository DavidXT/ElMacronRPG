using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private float speed = 1;
    private const float resetSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Condition en cas d'appuie sur une touche
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed += speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 1;//magic numbers
            }
            _animator.SetFloat("Vertical", speed);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed += speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 1;
            }
            _animator.SetFloat("Vertical", -speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed += speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 1;
            }
            _animator.SetFloat("Horizontal", -speed);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed += speed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 1;
            }
            _animator.SetFloat("Horizontal", speed);
        }

        //pourquoi est-ce que tu vérifies le LeftShit dans chacun alors que tu pourrais le faire au début ?

        //Reset sur l'idle
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D))
        {
            _animator.SetFloat("Horizontal", resetSpeed);
        }
        if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.S))
        {
            _animator.SetFloat("Vertical", resetSpeed);
        }
    }

}

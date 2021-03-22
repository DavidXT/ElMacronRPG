using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float speedDefault = 25;
    public Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speedDefault*2; //Vitesse de course
            //magic numbers
        }
        else
        {
            moveSpeed = speedDefault; //Vitesse set sur marche
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            speedDefault += 5; //Augmenter la speed du joueur si trop lent
            //je crois que tu as oublié ta commande de cheat
        }

        //Mouvement player
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, _rb.velocity.y, Input.GetAxis("Vertical")*moveSpeed * Time.deltaTime);
    }
}

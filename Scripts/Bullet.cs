using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 15;

    private void Start()
    {
        Destroy(gameObject, 100);//si la balle est toujours vivantes après 100 secondes, on la détruit
    }

    public void UpdateBullet()
    {
        //on la fait avancer toujours tout droit à vitesse constante
        transform.position += transform.forward * Time.deltaTime * _moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)//si elle entre en collision
    {
        if(collision.transform.GetComponent<AAI>())//avec un joueur
        {
            PlayersManagers.Instance.PlayerDied(collision.gameObject);//on tue le joueur
            Destroy(collision.gameObject);//on le détruit
        }

        BulletsManagers.Instance.RemoveBullet(gameObject);
        Destroy(gameObject);//on détruit la balle dans tout les cas
    }
}
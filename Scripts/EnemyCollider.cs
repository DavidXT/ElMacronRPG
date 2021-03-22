using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public Vector3 playerPos;
    public const float resetIdle = 0f;

    //Spawn des gameobjects sur la scene des combats
    public Vector3 playerFightpos = new Vector3(-13, 1, 5);
    public Vector3 enemyFightPos = new Vector3(-11.62f, 1.60f, 5.35f);

    //GetCamera Data
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera battleCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().enabled = false; //Empeche le joueur de bouger pendant le combat
            player.GetComponent<Animator>().SetFloat("Vertical", resetIdle); //Reset sur idle
            player.GetComponent<Animator>().SetFloat("Horizontal", resetIdle); //Reset sur idle
            player.GetComponent<AnimationHandler>().enabled = false; //Empeche les animations de mouvement du joueur pendant le combat
            mainCamera.gameObject.SetActive(false); //Disabled camera 
            battleCamera.gameObject.SetActive(true); //Switch camera

            player.transform.position = playerFightpos; //Tp joueur sur zone de combat
            player.transform.rotation = Quaternion.AngleAxis(90, Vector3.up); //Rotate joueur face à l'adversaire
            //magic numbers
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArcherAttack : MonoBehaviour{
    [SerializeField] private float range = 15f;
    [SerializeField] private float timeBetweenAttacks = 1f;
    private Animator anim;
    private GameObject player;
    private bool playerInRange;
    public float arrowSpeed = 800.0f;
    public Transform arrowSpawn;
    public Rigidbody arrowPrefab;
    Rigidbody clone;
    void Start(){
        arrowSpawn = GameObject.Find("ArrowSpawn").transform;
        anim = GetComponent<Animator>();
        player = GameManager.instance.Player;
    }
    void Update(){
        if(Vector3.Distance(transform.position,player.transform.position) < range){
            playerInRange = true;
            anim.SetTrigger("IsAttacking");
        }else{
            playerInRange = false;
        }
        //print("Player in arrow range "+playerInRange);
    }
    public void FireArcherProjectile(){
        clone = Instantiate(arrowPrefab,arrowSpawn.position, arrowSpawn.rotation) as Rigidbody;
        clone.AddForce(arrowSpawn.transform.right*arrowSpeed);
    }
}

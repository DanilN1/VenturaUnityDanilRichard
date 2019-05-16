using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;
public class SkeletonGruntMove : MonoBehaviour{
    [SerializeField] Transform player;
    private NavMeshAgent nav;
    private Animator anim;
    private SkeletonGruntHealth skeletonGruntHealth;
    void Awake(){
        Assert.IsNotNull(player);
    }
    void Start(){
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        skeletonGruntHealth = GetComponent<SkeletonGruntHealth>();
    }
    void Update(){
        if(Vector3.Distance(player.position,this.transform.position) < 6){
            if(!GameManager.instance.GameOver && skeletonGruntHealth.IsAlive){
                nav.SetDestination(player.position);
                 anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", false);
            }
        }else if(GameManager.instance.GameOver || !skeletonGruntHealth.IsAlive){
            anim.SetBool("isWalking",false);
            anim.SetBool("isIdle",true);
            nav.enabled = false;
        }
    }
}

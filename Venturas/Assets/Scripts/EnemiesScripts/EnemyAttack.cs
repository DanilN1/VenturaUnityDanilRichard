using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : MonoBehaviour{
    [SerializeField] private float range = 3f;
    [SerializeField] private float timeBetweenAttacks = 1f;
    private Animator anim;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider weaponCollider;
    private SkeletonGruntHealth skeletonGruntHealth;
    private AudioSource audio;
    public AudioClip attackAudio;
    void Start(){
        skeletonGruntHealth = GetComponent<SkeletonGruntHealth>();
        anim = GetComponent<Animator>();
        player = GameManager.instance.Player;
        weaponCollider = GetComponentInChildren<BoxCollider>();
        audio = GetComponent<AudioSource>();
        StartCoroutine(attack());
    }
    void Update(){
        if(Vector3.Distance(transform.position, player.transform.position) < range && skeletonGruntHealth.IsAlive){
            playerInRange = true;
        }else{
            playerInRange = false;
        }
    }
    IEnumerator attack(){
        if(playerInRange && !GameManager.instance.GameOver){
            audio.PlayOneShot(attackAudio);
            anim.Play("SkeletonGruntAttack");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
        yield return null;
        StartCoroutine(attack());
    }
}

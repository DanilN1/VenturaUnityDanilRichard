using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float timer = 0f;
    public float waitTime = 2.0f;
    public GameObject currentCheckpoint;
    private GameObject player;
    private PlayerHealth playerHealth;
    public PlayerHealth playerSlider;
    private CharacterMovement characterMovement;
    public Animator anim;
    private LifeManager lifeSystem;
    public void RespawnPlayer(){
        timer += Time.deltaTime;
        if(timer >= waitTime){
            lifeSystem.TakeLife();
            print("Player Respawn");
            player.transform.position = currentCheckpoint.transform.position;
            playerHealth.CurrentHealth = 100;
            timer = 0f;
            playerHealth.HealthSlider.value = playerHealth.CurrentHealth;
            characterMovement.enabled = true;
            anim.Play("Blend Tree");
        }
    }
    void Start(){
        player = GameManager.instance.Player;
        playerSlider = player.GetComponent<PlayerHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();
        characterMovement = player.GetComponent<CharacterMovement>();
        anim = player.GetComponent<Animator>();
        lifeSystem = FindObjectOfType<LifeManager>();
    }
    void Update(){
        
    }
}

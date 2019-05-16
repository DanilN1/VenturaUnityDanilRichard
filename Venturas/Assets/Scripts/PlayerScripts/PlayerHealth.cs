using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
public class PlayerHealth : MonoBehaviour{
    [SerializeField] int startingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2.0f;
    [SerializeField] Slider healthSlider;
    private CharacterMovement characterMovement;
    [SerializeField] private float  timer = 0f;
    private Animator anim;
    private AudioSource audio;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public AudioClip healthItemAudio;
    public LevelManager levelManager;
    public bool isDead;
    public int CurrentHealth{
        get{return currentHealth;}
        set{
            if(value < 0){
                currentHealth = 0;
            }else{
                currentHealth = value;
            }
        }
    }
    public Slider HealthSlider{
        get{return healthSlider;}
    }
    [SerializeField] private int currentHealth;
    void Awake(){
        Assert.IsNotNull(healthSlider);
    }
    public void KillBox(){
        CurrentHealth = 0;
        healthSlider.value = currentHealth;
    }
    public void PlayerKill(){
        if(currentHealth <= 0){
            characterMovement.enabled = false;
            levelManager.RespawnPlayer();
        }
    }
    void OnTriggerEnter(Collider other){
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver){
            if (other.tag == "Weapon"){
                takeHit();
                timer = 0;
            }
        }
    }
    void takeHit(){
        if(currentHealth > 0){
            GameManager.instance.PlayerHit(currentHealth);
            audio.PlayOneShot(hurtAudio);
            anim.Play ("Hurt");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
        }
        if(currentHealth <= 0){
            GameManager.instance.PlayerHit(currentHealth);
            audio.PlayOneShot(deathAudio);
            anim.SetTrigger("isDead");
            characterMovement.enabled = false;
        }
    }
    public void PowerUpHealth(){
        if (currentHealth <= 80){
            CurrentHealth += 20;
        }else if(currentHealth < startingHealth){
            CurrentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
        audio.PlayOneShot(healthItemAudio);
    }
    void Start(){
        anim = GetComponent <Animator>();
        currentHealth = startingHealth;
        characterMovement = GetComponent<CharacterMovement>();
        audio = GetComponent<AudioSource>();
        levelManager = FindObjectOfType<LevelManager>();
        isDead = false;
    }
    void Update(){
        timer += Time.deltaTime;
        PlayerKill();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour{
    public static bool GameIsPaused = false;
    public GameObject audioManager;
    private AudioSource audioSource;
    private AudioSource pauseAudio;
    public AudioClip pause;
    void Start(){
        audioSource = audioManager.GetComponent<AudioSource>();
        pauseAudio = GetComponent<AudioSource>();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.P)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }
    void Resume(){
        Time.timeScale = 1f;
        GameIsPaused = false;
        audioSource.Play();
        pauseAudio.PlayOneShot(pause);
    }
    void Pause(){
        Time.timeScale = 0f;
        GameIsPaused = true;
        audioSource.Pause();
        pauseAudio.PlayOneShot(pause);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public LevelManager levelManager;
    void Start(){
        levelManager = FindObjectOfType<LevelManager>();
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider other){
        if(other.name == "Player"){
            levelManager.currentCheckpoint = gameObject;
        }
    }
}

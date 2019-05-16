using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour{
    public GameObject[] items;
    int randomInt;
    public void Drop(){
        randomInt = Random.Range(0,items.Length);
        Instantiate(items[randomInt],transform.position,Quaternion.identity);
    }
    void Start(){
        
    }
    void Update(){
        
    }
}

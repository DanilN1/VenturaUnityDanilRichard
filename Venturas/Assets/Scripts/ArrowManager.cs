using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrowManager : MonoBehaviour{
    private int arrowCounter;
    private Text arrowText;
    private CharacterMovement characterMovement;
    private GameObject player;
    void Start(){
        arrowText = GetComponent<Text>();
        player = GameManager.instance.Player;
        characterMovement = player.GetComponent<CharacterMovement>();
        arrowCounter = characterMovement.arrowQuantity;
    }
    void Update(){
        arrowCounter = EqualizeAmmo(characterMovement.arrowQuantity);
        arrowText.text = "X " + arrowCounter;
    }
    public int EqualizeAmmo(int ammo){
        return arrowCounter = ammo;
    }
}

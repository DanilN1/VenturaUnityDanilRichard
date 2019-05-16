using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwordManager : MonoBehaviour{
    private int swordCounter;
    private Text swordText;
    private CharacterMovement characterMovement;
    private GameObject player;
    void Start(){
        swordText = GetComponent<Text>();
        player = GameManager.instance.Player;
        characterMovement = player.GetComponent<CharacterMovement>();
        swordCounter = characterMovement.swordQuantity;
    }
    void Update(){
        swordCounter = EqualizeAmmo(characterMovement.swordQuantity);
        swordText.text = "X " + swordCounter;
    }
    public int EqualizeAmmo(int ammo){
        return swordCounter = ammo;
    }
}

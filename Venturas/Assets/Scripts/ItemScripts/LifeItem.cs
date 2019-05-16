using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class LifeItem : MonoBehaviour{
    private GameObject player;
    private LifeManager lifeManager;
    private SpriteRenderer spriteRenderer;
    public GameObject pickUpEffect;
    private PowerItemExplode powerItemExplode;
    private BoxCollider boxCollider;
    private AudioSource audio;
    public AudioClip pickAudio;
    private float x;
    private float y;
    void Start(){
        player = GameManager.instance.Player;
        lifeManager = FindObjectOfType<LifeManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        powerItemExplode = GetComponent<PowerItemExplode>();
        boxCollider = GetComponent<BoxCollider>();
        audio = GetComponent<AudioSource>();
        x = boxCollider.transform.position.x;
        y = boxCollider.transform.position.y;
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            PickLife();
            InsertarAItemsObtenidos("Consumible", "Corazon", "Roja", x.ToString(), y.ToString(), "1");
        }
    }
    public void PickLife(){
        lifeManager.GiveLife();
        audio.PlayOneShot(pickAudio);
        powerItemExplode.Pickup();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        Destroy(gameObject);
    }
    void Update(){
        
    }


    static void InsertarAItemsObtenidos(string tipo, string descripcion, string color, string x, string y, string numeroPersonaje)
        {
            try
            {
                string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("dbo.InicializarItems", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@Tipo", tipo);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@Color", color);
                        command.Parameters.AddWithValue("@CoordenadaX", x);
                        command.Parameters.AddWithValue("@CoordenadaY", y);
                        command.Parameters.AddWithValue("@PERNumeroPersonaje", numeroPersonaje);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                print(e.ToString());

            }
        }
}

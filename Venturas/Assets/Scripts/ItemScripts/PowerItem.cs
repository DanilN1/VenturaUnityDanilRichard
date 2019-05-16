using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class PowerItem : MonoBehaviour{
    private GameObject player;
    private AudioSource audio;
    //private Invincible invincible;
    private PlayerHealth playerHealth;
    private ParticleSystem particleSystem;
    private MeshRenderer meshrenderer;
    private ParticleSystem brainParticles;
    private PowerItemExplode powerItemExplode;
    private SphereCollider sphereCollider;
    public GameObject pickupEffect;
    public AudioClip pickAudio;
    private float x;
    private float y;
    void Pickup(){
        Instantiate(pickupEffect,transform.position,transform.rotation);
    }
    void Start(){
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.enabled = true;
        particleSystem = player.GetComponent<ParticleSystem>();
        particleSystem.enableEmission = false;
        meshrenderer = GetComponentInChildren <MeshRenderer>();
        brainParticles = GetComponent<ParticleSystem>();
        powerItemExplode = GetComponent<PowerItemExplode>();
        sphereCollider = GetComponent<SphereCollider>();
        audio = GetComponent<AudioSource>();
        x = sphereCollider.transform.position.x;
        y = sphereCollider.transform.position.y;
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            StartCoroutine(InvincibleRoutine());
            meshrenderer.enabled = false;
            InsertarAItemsObtenidos("Booster", "Cerebro", "Crema", x.ToString(), y.ToString(), "1");
        }
    }
    public IEnumerator InvincibleRoutine(){
        print("pick invincible");
        audio.PlayOneShot(pickAudio);
        powerItemExplode.Pickup();
        particleSystem.enableEmission = true;
        playerHealth.enabled = false;
        brainParticles.enableEmission = false;
        sphereCollider.enabled = false;
        yield return new WaitForSeconds(10f);
        print("no more invincible");
        particleSystem.enableEmission = false;
        playerHealth.enabled = true;
        Destroy(gameObject);
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

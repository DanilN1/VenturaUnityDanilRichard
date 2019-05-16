using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using UnityEngine;

public class SkeletonMageHealth : MonoBehaviour{
    [SerializeField] private float dissapearSpeed = 2f;
    [SerializeField] private int startingHealth = 10;
    [SerializeField] private int currentHealth;
    private bool isAlive;
    private float timer = 0f;
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private Animator anim;
    private AudioSource audio;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    private bool dissapearEnemy = false;
    private int numeroPersonaje = 1;
    private int numeroEnemigo = 3;
    public bool IsAlive{
        get{return isAlive;}
    }
    private DropItem dropItem;
    void Start(){
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;
        dropItem = GetComponent<DropItem>();
        audio = GetComponent<AudioSource>();
    }
    void Update(){
        timer += Time.deltaTime;
        if(dissapearEnemy){
            transform.Translate(-Vector3.up*dissapearSpeed*Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "SwordWeapon"){
            InsertarAEnemigoAsesinado(DateTime.Now.ToString(), numeroPersonaje.ToString(), numeroEnemigo.ToString(), "Sword", "Lanzamiento arrojable");
            takeHit();
            timer = 0f;
        }

        if(other.tag == "ArrowWeapon"){
            InsertarAEnemigoAsesinado(DateTime.Now.ToString(), numeroPersonaje.ToString(), numeroEnemigo.ToString(), "Arrow", "Lanzamiento proyectil");
            takeHit();
            timer = 0f;
        }
    }
    void takeHit(){
        if(currentHealth > 0){
            audio.PlayOneShot(hurtAudio);
            anim.Play("Hurt");
            currentHealth -= 10;
        }
        if(currentHealth <= 0){
            isAlive = false;
            KillEnemy();
        }
    }
    void KillEnemy(){
        capsuleCollider.enabled = false;
        audio.PlayOneShot(deathAudio);
        anim.SetTrigger("Death");
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(removeEnemy());
        dropItem.Drop();
    }
    IEnumerator removeEnemy(){
        yield return new WaitForSeconds(2f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    static void InsertarAEnemigoAsesinado(string time, string numeroPersonaje, string numeroEnemigo, string arma, string descripcion)
        {
            try
            {
                string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand("dbo.InicializarEnemigoAsesinado", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@ArmaAsesina", arma);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@FechaHora", time);
                        command.Parameters.AddWithValue("@PERNumeroPersonaje", numeroPersonaje);
                        command.Parameters.AddWithValue("@ENENumeroEnemigo", numeroEnemigo);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                /*IMPRIMIR MENSAJE DE ERROR
                 * Console.WriteLine(e.ToString());
                */
            }
        }
}

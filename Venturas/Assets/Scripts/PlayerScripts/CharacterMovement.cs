using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Data;
using System.Data.SqlClient;
using System;
public class CharacterMovement : MonoBehaviour{
    public float maxSpeed = 6.0f;
    public bool facingRight = true;
    public float moveDirection;
    new Rigidbody rigidbody;
    public float jumpSpeed = 300.0f;
    private Animator anim;
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadious = 0.2f;
    public LayerMask whatIsGround;
    public float swordSpeed = 600.0f;
    public int swordQuantity;
    public Transform swordSpawn;
    public Rigidbody swordPrefab;
    private AudioSource audio;
    public AudioClip jumpAudio;
    public AudioClip attackAudio;
    private ParticleSystem particleSystem;
    public float arrowSpeed = 800.0f;
    public int arrowQuantity;
    public Rigidbody arrowPrefab;
    private float timer = 0;
    private GameObject player;
    Rigidbody clone;
    private float x;
    private float y;
    private int numeroPersonaje = 1;
    int valor;
    public GameObject marcador;
    void Awake(){
        groundCheck = GameObject.Find("GroundCheck").transform;
        swordSpawn = GameObject.Find("SwordSpawn").transform;
    }
    void Start(){
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.enableEmission = false;
        player = GameManager.instance.Player;
        swordQuantity = Function1("2");
        arrowQuantity = Function1("1");
        //print(player.transform.position.x);
    }
    void Update(){
        timer += Time.deltaTime;
        x = player.transform.position.x;
        y = player.transform.position.y;
        if(timer > 10){
            InsertarAEstadoJugador(x.ToString(),y.ToString(),DateTime.Now.ToString());
            InsertarAPersonajeEstado(numeroPersonaje);
            timer = 0;
        }
        if(Input.GetButtonDown("Marcador")){
            marcador.SetActive(true);
        }
        if(Input.GetButtonUp("Marcador")){
            marcador.SetActive(false);
        }
        moveDirection = Input.GetAxis("Horizontal");
        if(grounded && Input.GetButtonDown("Jump")){
            anim.SetTrigger("isJumping");
            audio.PlayOneShot(jumpAudio);
            rigidbody.AddForce(new Vector2(0,jumpSpeed));
        }
        if(Input.GetButtonDown("Fire1") && swordQuantity > 0){
            Attack();
        }
        if(Input.GetButtonDown("Fire2") && arrowQuantity > 0){
            ArrowAttack();
        }
        if(moveDirection > 0.0f && !facingRight){
            Flip();
        }else if(moveDirection < 0.0f && facingRight){
            Flip();
        }
        anim.SetFloat("Speed", Mathf.Abs(moveDirection));
    }
    private void FixedUpdate(){
        grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadious,whatIsGround);
        rigidbody.velocity = new Vector2(moveDirection * maxSpeed, rigidbody.velocity.y);
    }
    void Flip(){
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
    public void CallFireProjectile(){
        clone = Instantiate(swordPrefab,swordSpawn.position,swordSpawn.rotation) as Rigidbody;
        clone.AddForce(swordSpawn.transform.right * swordSpeed);
        swordQuantity--;
        InsertarARecurso("Arrojable","2",numeroPersonaje.ToString());
    }
    public void CallFireProjectile2(){
        clone = Instantiate(arrowPrefab,swordSpawn.position,swordSpawn.rotation) as Rigidbody;
        clone.AddForce(swordSpawn.transform.right * arrowSpeed);
        arrowQuantity--;
        InsertarARecurso("Proyectil","1",numeroPersonaje.ToString());
    }
    void Attack(){
        audio.PlayOneShot(attackAudio);
        anim.SetTrigger("attacking");
    }
    void ArrowAttack(){
        anim.SetTrigger("attacking2");
    }
    static void InsertarAEstadoJugador(string x,string y,string time){
        try{
            string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";
            using (SqlConnection connection = new SqlConnection(conn)){   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.InicializarEstadoJugador", connection)){
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@FechaHora", time);
                    command.Parameters.AddWithValue("@CoordenadaX", x);
                    command.Parameters.AddWithValue("@CoordenadaY", y);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e){
            print(e.ToString());
        }
    }
    static void InsertarAPersonajeEstado(int numero){
        try{
            string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";
            using (SqlConnection connection = new SqlConnection(conn)) {  //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.InicializarPersonajeEstado", connection)){
                command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                command.Parameters.AddWithValue("@PERNumeroPersonaje", numero);
                command.ExecuteNonQuery();
            }
        }
    }
    catch (SqlException e){
        print(e.ToString());
    }
    }
    static void InsertarARecurso(string tipo,string municion,string personaje){
        try{
            string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";
            using (SqlConnection connection = new SqlConnection(conn)){   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.InicializarRecurso", connection)){
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@Tipo", tipo);
                    command.Parameters.AddWithValue("@MUNNumeroMunicion", municion);
                    command.Parameters.AddWithValue("@PERNumeroPersonaje", personaje);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e){
            print(e.ToString());
        }
    }

    int Function1(string numeroMunicion)
        {
            try
            {
                string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT dbo.ScalarFunction(@X)", connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand

                        command.Parameters.AddWithValue("@X", numeroMunicion);


                        valor = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException e)
            {
                print(e.ToString());
            }

            return valor;
        }
}

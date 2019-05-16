using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class SwordItem : MonoBehaviour{
    private GameObject player;
    private CharacterMovement characterMovement;
    private BoxCollider boxCollider;
    private float x;
    private float y;
    void Start(){
        player = GameManager.instance.Player;
        characterMovement = player.GetComponent<CharacterMovement>();
        boxCollider = GetComponent<BoxCollider>();
        x = boxCollider.transform.position.x;
        y = boxCollider.transform.position.y;
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            AddSwords();
            InsertarAItemsObtenidos("Municion", "Espada", "Gris", x.ToString(), y.ToString(), "1");
        }
    }
    void AddSwords(){
        characterMovement.swordQuantity += 10;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using System.Data.SqlClient;
public class TableText : MonoBehaviour{
    private Text texto;
    private String[][] arreglo;
    void Start(){
        texto = GetComponent<Text>();
        
    }
    void Update(){
        Function2(texto);
    }
    static void Function2(Text texto)
        {
            try
            {
                string conn = @"Data Source = 127.0.0.1; user id = FundacionEImperio; password = ProyectoFinal123; Initial Catalog = FundacionEImperio;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Function2()", connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int c = 0;
                            while (reader.Read())
                            {
                                c++;
                                texto.text = texto.text + reader.GetInt32(0).ToString() +"\t\t"+ reader.GetString(1) +"\t\t"+ reader.GetString(2) +"\t"+ reader.GetString(3) + "\n";
                            }
                            print(c);
                        }
                        //Console.WriteLine("Lista la funcion con {0}", valor);
                    }

                }
            }
            catch (SqlException e)
            {
                print(e.ToString());
            }
        }
}

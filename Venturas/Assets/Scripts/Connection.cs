using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
public class Connection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

            try
            {
                //código muy chafa, solo para que entiendan cómo se hacen las conexiones , uso de vistas y funciones
                //string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

                string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;User Id=Ventura; Password = Ventura12345";

                //("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                { //https://stackoverflow.com/questions/4717789/in-a-using-block-is-a-sqlconnection-closed-on-return-or-exception
                    connection.Open();
                    //("Done.");

                    //NEVER DO THIS IN PRODUCTION//   //En serio, les tumbo 40 puntos en el proyecto final, pueden usarlo para probar y en ese mismo momento lo borran/comentan
                    //es bien fácil que se hagan SQL INJECTION al hacerlo así
                    //ver (tarea)
                    //https://www.youtube.com/watch?v=WFFQw01EYHM
                    string sql = "INSERT INTO TablaDemo(Nombre,Matricula) VALUES('Carlos', 'L007777')";//nunca van a hacer esto
                    string sql2 = "SELECT * FROM TablaDemo"; //esto tampoco
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        //("Done.");
                    }
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //(e.ToString());
            }


            //Accediendo la info con vistas
            try
            {
                string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    //("Done.");

                    string sql = "SELECT * FROM ViewDeTablaDemo"; //ya se ve más bonito


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        //command.Parameters.Add()
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //(e.ToString());
            }

            //Vistas con parámetros
            try
            {
                string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    //("Done.");

                    string sql = "SELECT * FROM ViewDeTablaDemo WHERE Nombre = @nombre"; //ya se ve más bonito


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                        command.Parameters.Add(new SqlParameter("nombre", "Ventura"));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //(e.ToString());
            }

            //STORE PROCEDURES
            try
            {
                string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();
                    //("Done.");


                    using (SqlCommand command = new SqlCommand("dbo.InsertarDatos", connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                        command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                        command.Parameters.AddWithValue("@Nombre", "Molina");
                        command.Parameters.AddWithValue("@Matricula", "2222222222");

                        command.ExecuteNonQuery();
                        //("Lista escritura con un SP.");
                    }
                }
            }
            catch (SqlException e)
            {
                //(e.ToString());
            }


            //FUNCIONES (una escalar) (no olvidar que hay 3 tipos)
            try
            {
                string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT dbo.ElCubo(@X)", connection))
                    { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand

                        command.Parameters.AddWithValue("@X", 2);


                        int valor = (int)command.ExecuteScalar();
                        //("Lista la funcion con {0}", valor);
                    }
                }
            }
            catch (SqlException e)
            {
                //(e.ToString());
            }






            //("All done. Press any key to finish...");
            //.ReadKey(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



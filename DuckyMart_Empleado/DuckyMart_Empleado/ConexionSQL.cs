using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace DuckyMart_Empleado
{
    public class ConexionSQL
    {
        bool Exito = false;
        public SqlConnection conexion = new SqlConnection("user id = sa; password=admin;server=25.10.112.128\\SQLEXPRESS;database=DUCKYMARTBD;");

        #region Iniciar y Cerrar Conexion
        public bool Conexion(bool Iniciar)
        {
            Exito = false;
            switch (Iniciar)
            {
                case true:
                    try
                    {
                        conexion.Open();
                        Exito = true;

                    }
                    catch (Exception e)
                    {
                        Exito = false;
                    }
                    break;

                case false:
                    try
                    {
                        conexion.Close();
                        Exito = true;
                    }
                    catch (Exception)
                    {
                        Exito = false;
                    }
                    break;
            }
            return Exito;
        }
        #endregion
    }
}

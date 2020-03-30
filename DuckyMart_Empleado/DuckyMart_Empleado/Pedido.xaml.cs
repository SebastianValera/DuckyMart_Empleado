using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DuckyMart_Empleado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pedido : ContentPage
    {
        ConexionSQL BD = new ConexionSQL();
        int idPedido;
        string nombreEstatus;
        bool cargado = true;
        SqlCommand cmd;
        List<Estatus> listaEstatus;

        public Pedido(Ordenes pedidoOrden)
        {
            InitializeComponent();
            this.idPedido = pedidoOrden.IdPedido;
            this.CodPedido.Text += pedidoOrden.CodPedido;
            this.Fecha.Text += pedidoOrden.Fecha;
            nombreEstatus = pedidoOrden.Estatus;
            this.PrecioTotal.Text += pedidoOrden.PrecioTotal.ToString();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            ObtenerEstatus();
            for (int i = 0; i < listaEstatus.Count; i++)
            {
                if (listaEstatus[i].Nombre == nombreEstatus)
                {
                    comboEstatus.SelectedIndex = i;
                    cargado = false;
                    break;
                }
            }
            ObtenerProductos();
        }

        public void ObtenerEstatus()
        {
            if (BD.Conexion(true))
            {
                // Se realiza la conexión a la BD
                cmd = BD.conexion.CreateCommand();
                SqlDataAdapter Adaptador = new SqlDataAdapter();
                var Data = new DataTable();

                // Se estructura el query
                cmd.CommandText = "Select\n" +
                    "id_estatus,\n" +
                    "nombre\n" +
                    "FROM Estatus";

                cmd.ExecuteNonQuery(); // Se ejecuta

                // Se crea un adaptador de sql, guardará el data source que contiene la información de la consulta
                Adaptador.SelectCommand = cmd;
                Adaptador.Fill(Data);

                listaEstatus = new List<Estatus>();
                Estatus estatus;

                for (int i = 0; i < Data.Rows.Count; i++)
                {
                    estatus = new Estatus();
                    estatus.IdEstatus = Convert.ToInt32(Data.Rows[i]["id_estatus"]);
                    estatus.Nombre = Data.Rows[i]["nombre"].ToString();
                    listaEstatus.Add(estatus);
                }

                foreach (Estatus item in listaEstatus)
                {
                    comboEstatus.Items.Add(item.Nombre);
                }

                BD.Conexion(false);

            }
        }

        public void ObtenerProductos()
        {
            if (BD.Conexion(true))
            {
                // Se realiza la conexión a la BD
                cmd = BD.conexion.CreateCommand();
                SqlDataAdapter Adaptador = new SqlDataAdapter();
                var Data = new DataTable();

                // Se estructura el query
                cmd.CommandText = "Select\n" +
                    "id_propedido,\n" +
                    "PRODUCTO.foto,\n" +
                    "PRODUCTO.nombre,\n" +
                    "PRODUCTO.precio, \n" +
                    "cantidad \n" +
                    "FROM PRODUCTOSPEDIDO \n" +
                    "INNER JOIN PRODUCTO \n" +
                    "ON PRODUCTO.id_producto = PRODUCTOSPEDIDO.id_producto \n" +
                    "WHERE PRODUCTOSPEDIDO.id_pedido = " + idPedido;
                cmd.ExecuteNonQuery(); // Se ejecuta

                // Se crea un adaptador de sql, guardará el data source que contiene la información de la consulta
                Adaptador.SelectCommand = cmd;
                Adaptador.Fill(Data);

                List<Productos> productos = new List<Productos>();
                for (int i = 0; i < Data.Rows.Count; i++)
                {
                    Productos producto = new Productos();
                    producto.IdProPedido = Convert.ToInt32(Data.Rows[i]["id_propedido"]);
                    producto.Bytesfoto = (byte[])Data.Rows[i]["foto"];
                    producto.Nombre = Data.Rows[i]["nombre"].ToString();
                    producto.Precio = Convert.ToDecimal(Data.Rows[i]["precio"]);
                    producto.Cantidad = Convert.ToInt32(Data.Rows[i]["cantidad"]);
                    productos.Add(producto);
                }
                this.ListaProductos.ItemsSource = productos;
                BD.Conexion(false);
            }
        }

        private void comboEstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargado == false && BD.Conexion(true))
            {
                // Se realiza la conexión a la BD
                cmd = BD.conexion.CreateCommand();
                SqlDataAdapter Adaptador = new SqlDataAdapter();
                var Data = new DataTable();

                // Se estructura el query
                cmd.CommandText = "UPDATE PEDIDO\n" +
                    "SET id_estatus =  '" + listaEstatus[comboEstatus.SelectedIndex].IdEstatus + "'\n" +
                    "WHERE PEDIDO.id_pedido = " + idPedido;
                cmd.ExecuteNonQuery(); // Se ejecuta

                BD.Conexion(false);
            }
        }
    }
}
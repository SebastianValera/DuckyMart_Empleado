using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DuckyMart_Empleado
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ConexionSQL BD = new ConexionSQL();
        List<Ordenes> pedidos;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            ObtenerPedidos();
        }

        public void ObtenerPedidos(string codOrden = "") {
            if (BD.Conexion(true)) {
                // Se realiza la conexión a la BD y se instancia un objeto de la misma, además se declaran variables
                SqlCommand cmd = BD.conexion.CreateCommand();
                SqlDataAdapter Adaptador = new SqlDataAdapter();
                var Data = new DataTable();

                // Se estructura el query
                cmd.CommandText = "Select\n" +
                    "id_pedido,\n" +
                    "codPedido,\n" +
                    "ESTATUS.nombre,\n" +
                    "precioTotal, \n" +
                    "fechaPedido \n" +
                    "From PEDIDO \n" +
                    "INNER JOIN ESTATUS \n" +
                    "ON ESTATUS.id_estatus = PEDIDO.id_estatus";
                
                if (codOrden != "" && codOrden != null)
                {
                    cmd.CommandText += "\nWHERE codPedido = '" + codOrden + "'";
                }
                
                cmd.ExecuteNonQuery(); // Se ejecuta

                // Se crea un adaptador de sql, guardará el data source que contiene la información de la consulta
                Adaptador.SelectCommand = cmd;
                Adaptador.Fill(Data);

                pedidos = new List<Ordenes>();
                for (int i = 0; i < Data.Rows.Count; i++)
                {
                    Ordenes pedido = new Ordenes();
                    pedido.IdPedido = Convert.ToInt32(Data.Rows[i]["id_pedido"]);
                    pedido.CodPedido = Data.Rows[i]["codPedido"].ToString();
                    pedido.Estatus = Data.Rows[i]["nombre"].ToString();
                    pedido.PrecioTotal = Convert.ToDecimal(Data.Rows[i]["precioTotal"]);
                    pedido.Fecha = Convert.ToDateTime(Data.Rows[i]["fechaPedido"]).ToString("MM/dd/yyyy");
                    pedidos.Add(pedido);
                }
                this.ListaPedidos.ItemsSource = pedidos;
                BD.Conexion(false);
            }            
        }

        async void ListaPedidos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Pedido(pedidos[e.ItemIndex]));
            ObtenerPedidos();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ObtenerPedidos(EDT_Codigo.Text);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            ObtenerPedidos(EDT_Codigo.Text);
            this.ListaPedidos.EndRefresh();
        }
    }
}

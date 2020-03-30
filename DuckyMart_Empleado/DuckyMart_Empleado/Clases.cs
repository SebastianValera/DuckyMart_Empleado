using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DuckyMart_Empleado
{
    public class Estatus
    {
        int idEstatus;
        string nombre;

        public int IdEstatus { get => idEstatus; set => idEstatus = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }

    public class Productos
    {
        int idProPedido;
        byte[] bytesfoto;
        string nombre;
        decimal precio;
        int cantidad;

        public int IdProPedido { get => idProPedido; set => idProPedido = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public decimal Precio { get => precio; set => precio = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }

        public byte[] Bytesfoto { get => bytesfoto; set => bytesfoto = value; }

        public ImageSource Foto { get => ImageSource.FromStream(() => new MemoryStream(this.bytesfoto));}
    }

    public class Ordenes
    {
        int idPedido;
        string codPedido;
        string estatus;
        decimal precioTotal;
        string fecha;

        public int IdPedido { get => idPedido; set => idPedido = value; }
        public string CodPedido { get => codPedido; set => codPedido = value; }
        public string Estatus { get => estatus; set => estatus = value; }
        public decimal PrecioTotal { get => precioTotal; set => precioTotal = value; }
        public string Fecha { get => fecha; set => fecha = value; }
    }
}

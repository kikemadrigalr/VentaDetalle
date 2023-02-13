using System.Reflection.Metadata.Ecma335;

namespace VentaDetalle.Models
{
    public class DetalleVenta
    {
        public int IdVentaDetalle { get; set; }

        public string Producto { get; set; }

        public decimal Precio { get; set;}

        public int Cantidad { get; set; }

        public decimal Total { get; set; }
    }
}

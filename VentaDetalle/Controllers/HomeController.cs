using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VentaDetalle.Models;

using System.Xml.Linq;
using System.Data;
using Microsoft.Data.SqlClient;

namespace VentaDetalle.Controllers
{
    public class HomeController : Controller
    {
        private readonly string SqlCadena;

        //constructor se utiliza para obtener la cadena de conexion a la BD
        public HomeController(IConfiguration config)
        {
            SqlCadena = config.GetConnectionString("SqlCadena");
        }

        public IActionResult Index()
        {
            return View();
        }

        //metodo que recibe el json con los datos creados en el javascript
        //se tratara como un XML en este metodo
        [HttpPost]
        public JsonResult GuardarVenta([FromBody] Venta body)
        {
            //estructura del XML con la informacion de la venta
            XElement venta = new XElement("Venta",
                new XElement("NumeroDocumento", body.NumeroDocumento),
                new XElement("RazonSocial", body.RazonSocial),
                new XElement("Total", body.Total)
                );

            //estructura del XML con la informacion del detalle de la venta
            XElement detalleVenta = new XElement("DetalleVenta");

            foreach(DetalleVenta item in body.ListVentaDetalle)
            {
                detalleVenta.Add(new XElement("Item", 
                    new XElement("Producto", item.Producto),
                    new XElement("Precio", item.Precio),
                    new XElement("Cantidad", item.Cantidad),
                    new XElement("Total", item.Total)
                    ));
            }

            //agregar el detalle de venta a la venta
            venta.Add(detalleVenta);

            //conectar a la base de datos y enviar la informacion del xml
            using (SqlConnection conexion = new SqlConnection(SqlCadena))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_GuardarVenta", conexion);
                cmd.Parameters.Add("venta_xml", SqlDbType.Xml).Value= venta.ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }

                return Json(new { respuesta = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
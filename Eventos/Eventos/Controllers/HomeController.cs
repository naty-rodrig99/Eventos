using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Eventos.Models;
using System.Data;
using System.Diagnostics;

namespace Eventos.Controllers
{
    public class HomeController : Controller
    {
        private ESeventosEntities db = new ESeventosEntities();
        public static int idCliente;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Administrador()
        {
            return View();
        }

        public ActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MenuAdmin(Login login)
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "exec SP_Login_Administrador " + login.usuario + ", '" + login.contrasena + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            string getValue = cmd.ExecuteScalar().ToString();

            if (getValue.Equals("1"))
            {
                return View();
            }
            return View("~/Views/Home/Administrador.cshtml");
        }

        [HttpPost]
        public ActionResult MenuCliente(Login login)
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "exec SP_Login_Cliente " + login.usuario + ", '" + login.contrasena + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            string getValue = cmd.ExecuteScalar().ToString();

            if (getValue.Equals("1"))
            {
                SqlConnection conn1 = new SqlConnection(connection);
                String query1 = "exec SP_EncuentraCliente " + login.usuario + ", '" + login.contrasena + "'";
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                conn1.Open();
                string getId = cmd1.ExecuteScalar().ToString();
                idCliente = Convert.ToInt16(getId);
                return View();
            }
            return View("~/Views/Home/Cliente.cshtml");
        }

        public ActionResult RegistrarCliente()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCliente([Bind(Include = "usuario,correo,cedula,contrasena")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Cliente");
            }

            return View(cliente);
        }

        public ActionResult RegistrarAdministrador()
        {
            return View();
        }

        // POST: Administrador/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarAdministrador([Bind(Include = "usuario,correo,cedula,contrasena")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                db.Administradors.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("MenuAdmin");
            }
            return View(administrador);
        }


       
        public ActionResult HistorialReservasLocal()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Local"; ;
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }

        [HttpGet]
        public ActionResult HistorialReservasCatering()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Catering"; ;
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }

        [HttpGet]
        public ActionResult HistorialReservasMusica()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Musica";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }

        [HttpGet]
        public ActionResult HistorialReservasDecoracion()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Decoracion";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }

        [HttpGet]
        public ActionResult VerRecursos()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_VerRecursos";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }


        ///////////////////// CLIENTE ////////////////////////////////

        [HttpGet]
        public ActionResult HistorialReservasCliente()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Cliente";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }

        public ActionResult FiltrarPaquetesLugar(string search)
        {
            var lugar = from c in db.Paquetes select c;
            if (!string.IsNullOrEmpty(search))
            {
                lugar = lugar.Where(c => c.lugar == search);
            }
            return View(lugar);
        }

        public ActionResult FiltrarPaquetesPrecio(string search)
        {
            var precio = from c in db.Paquetes select c;
            if (!string.IsNullOrEmpty(search))
            {
                precio = precio.Where(c => c.precio == search);
            }
            return View(precio);
        }


    }
}
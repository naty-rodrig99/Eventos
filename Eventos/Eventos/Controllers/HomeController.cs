using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Eventos.Models;
using System.Data;

namespace Eventos.Controllers
{
    public class HomeController : Controller
    {
        private ESeventosEntities db = new ESeventosEntities();

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


        [HttpPost]
        public ActionResult HistorialReservas()
        {
            String connection = "Data Source=DESKTOP-SV37236\\NATALIASQL;Initial Catalog=ESeventos;Persist Security Info=True;User ID=sa;Password=naty0409;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "SP_Historial_Reservaciones_Local"; 
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            return View(dt);
        }


    }
}
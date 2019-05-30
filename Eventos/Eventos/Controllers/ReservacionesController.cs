using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventos.Models;

namespace Eventos.Controllers
{
    public class ReservacionesController : Controller
    {
        private ESeventosEntities1 db = new ESeventosEntities1();

        // GET: Reservaciones
        public ActionResult Index()
        {
            return View(db.Reservacions.ToList());
        }

        // GET: Reservaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservacions.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // GET: Reservaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservacion reservacion)
        {
            String connection = "Data Source=DESKTOP-TK36QCF;Initial Catalog=ESeventos;Persist Security Info=True;User ID=creador;Password=309808;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "exec SP_RealizarReservacion " + reservacion.numReservacion + ", '" + reservacion.fecha + reservacion.hora + ", '" + reservacion.descripcion + ", '" + reservacion.idPaquete + ", '" + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            return View("~/Views/Reservaciones/Index.cshtml");
        }

        // GET: Reservaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservacions.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // POST: Reservaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idReservacion,numReservacion,fecha,hora,descripcion")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservacion);
        }

        // GET: Reservaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservacions.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // POST: Reservaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservacion reservacion = db.Reservacions.Find(id);
            db.Reservacions.Remove(reservacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Reservar(Reservacion reservacion )
        {
            String connection = "Data Source=DESKTOP-TK36QCF;Initial Catalog=ESeventos;Persist Security Info=True;User ID=creador;Password=309808;MultipleActiveResultSets=True;Application Name=EntityFramework";
            SqlConnection conn = new SqlConnection(connection);
            String query = "exec SP_RealizarReservacion " + reservacion.numReservacion + ", '" + reservacion.fecha +reservacion.hora + ", '" + reservacion.descripcion + ", '" + reservacion.idPaquete+ ", '" + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            return View("~/Views/Home/Administrador.cshtml");
        }
    }
}

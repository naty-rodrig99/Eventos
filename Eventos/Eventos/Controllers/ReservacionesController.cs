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
using System.Net.Mail;

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
            String connection = "Server =.; Database = ESeventos; Trusted_Connection = True";
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
            String connection = "Server=.;Database=ESeventos;Trusted_Connection=True";
            SqlConnection conn = new SqlConnection(connection);
            String query = "exec SP_RealizarReservacion " + reservacion.numReservacion + ", '" + reservacion.fecha +reservacion.hora + ", '" + reservacion.descripcion + ", '" + reservacion.idPaquete+ ", '" + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            correoReservaCliente(HomeController.mail, (int) reservacion.idPaquete, reservacion.idReservacion, reservacion.descripcion);
            return View("~/Views/Home/Administrador.cshtml");
        }

        public void correoReservaCliente(String mail,int idPaquete,int idReserva, String descripcion)
        {

            MailMessage correo = new MailMessage("noreply@es-eventos.com", mail);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";
            correo.Subject = "Notificacion de reserva.";
            String body = "Estimado cliente, le informamos que se ha reservado un paquete a su nombre con los siguientes datos:\n\nNumero de reservacion:" + idReserva + "\n\nID del Paquete:" + idPaquete + "\n\nDescripcion:" + descripcion+"\n\n";
            body += "Si usted no realizo esta reservacion , favor ponerse en contacto con soporte tecnico.\nSaludos!\n\nEl Equipo de E|S Eventos";
            correo.Body = body;
            client.Send(correo);
        }
    }
}

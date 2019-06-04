using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Eventos.Models;

namespace Eventos.Controllers
{
    public class ReservacionsController : Controller
    {
        private ESeventosEntities1 db = new ESeventosEntities1();

        // GET: Reservacions
        public ActionResult Index()
        {
            return View(db.Reservacions.ToList());
        }

        // GET: Reservacions/Details/5
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

        // GET: Reservacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idReservacion,numReservacion,fecha,hora,descripcion,idPaquete")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                String connection = "Server=.;Database=ESeventos;Trusted_Connection=True";
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_RealizarReservacion", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@numReservacion", SqlDbType.VarChar).Value = reservacion.numReservacion;
                cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = reservacion.fecha;
                cmd.Parameters.Add("@hora", SqlDbType.Time).Value = reservacion.hora;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = reservacion.descripcion;
                cmd.Parameters.Add("@idPaquete", SqlDbType.Int).Value = reservacion.idPaquete;
                cmd.ExecuteNonQuery();
                //String query = "exec SP_RealizarReservacion "+reservacion.numReservacion+ "," +reservacion.fecha+ ","+ reservacion.hora + "," +"'"+ reservacion.descripcion + "'," + reservacion.idPaquete ;
                conn.Close();
                db.SaveChanges();
                correoReservaCliente(HomeController.mail,(int)reservacion.idPaquete, reservacion.idReservacion, reservacion.descripcion,1);
                return RedirectToAction("Index");
            }

            return View(reservacion);
        }

        // GET: Reservacions/Edit/5
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

        // POST: Reservacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idReservacion,numReservacion,fecha,hora,descripcion,idPaquete")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservacion).State = EntityState.Modified;
                db.SaveChanges();
                correoReservaCliente(HomeController.mail, (int)reservacion.idPaquete, reservacion.idReservacion, reservacion.descripcion, 2);
                return RedirectToAction("Index");
            }
            return View(reservacion);
        }

        // GET: Reservacions/Delete/5
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

        // POST: Reservacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservacion reservacion = db.Reservacions.Find(id);
            db.Reservacions.Remove(reservacion);
            db.SaveChanges();
            correoElimina(HomeController.mail, reservacion.idReservacion,reservacion.descripcion);
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


        public void correoReservaCliente(String mail, int idPaquete, int idReserva, String descripcion,int tipo)
        {

            MailMessage correo = new MailMessage("noreplyeseventos@gmail.com", mail);
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials= new System.Net.NetworkCredential("noreplyeseventos@gmail.com","eventos123");
            client.Port = 25;
            client.EnableSsl=true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "smtp.gmail.com";
            if (tipo == 1)
            {
                correo.Subject = "Notificacion de reserva.";
                String body = "Estimado cliente, le informamos que se ha reservado un paquete a su nombre con los siguientes datos:\n\nNumero de reservacion:" + idReserva + "\n\nID del Paquete:" + idPaquete + "\n\nDescripcion:" + descripcion + "\n\n";
                body += "Si usted no realizo esta reservacion , favor ponerse en contacto con soporte tecnico.\nSaludos!\n\nEl Equipo de E|S Eventos";
                correo.Body = body;
                client.Send(correo);
            }
            if(tipo == 2)
            {
                correo.Subject = "Notificacion de Edicion.";
                String body = "Estimado cliente, le informamos que se ha editado una reservacion a su nombre con los siguientes datos:\n\nNumero de reservacion:" + idReserva + "\n\nID del Paquete:" + idPaquete + "\n\nDescripcion:" + descripcion + "\n\n";
                body += "Si usted no edito esta reservacion , favor ponerse en contacto con soporte tecnico.\nSaludos!\n\nEl Equipo de E|S Eventos";
                correo.Body = body;
                client.Send(correo);
            }
        }
        public void correoElimina(String mail, int idReserva,String detalle)
        {

            MailMessage correo = new MailMessage("noreplyeseventos@gmail.com", mail);
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("noreplyeseventos@gmail.com", "eventos123");
            client.Port = 25;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "smtp.gmail.com";
          
            correo.Subject = "Notificacion de reserva.";
            String body = "Estimado cliente, lamentamos que haya cancelado la reservacion con el numero :" + idReserva+" ,que se detalla como:"+detalle+ "\n\n";
            body += "Si usted no cancelo la reservacion , favor ponerse en contacto con soporte tecnico.\nSaludos!\n\nEl Equipo de E|S Eventos";
            correo.Body = body;
            client.Send(correo);
            
        }
    }
}

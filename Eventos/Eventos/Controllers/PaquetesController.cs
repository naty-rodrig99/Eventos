using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventos.Models;

namespace Eventos.Controllers
{
    public class PaquetesController : Controller
    {
        private ESeventosEntities1 db = new ESeventosEntities1();

        // GET: Paquetes
        public ActionResult Index()
        {
            var paquetes = db.Paquetes.Include(p => p.ReservacionXClienteXPaquete);
            return View(paquetes.ToList());
        }

        // GET: Paquetes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquetes.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            return View(paquete);
        }

        // GET: Paquetes/Create
        public ActionResult Create()
        {
            ViewBag.idPaquete = new SelectList(db.ReservacionXClienteXPaquetes, "idReservacionXClienteXPaquete", "idReservacionXClienteXPaquete");
            return View();
        }

        // POST: Paquetes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPaquete,nombre,precio,lugar,disponible")] Paquete paquete)
        {
            if (ModelState.IsValid)
            {
                db.Paquetes.Add(paquete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idPaquete = new SelectList(db.ReservacionXClienteXPaquetes, "idReservacionXClienteXPaquete", "idReservacionXClienteXPaquete", paquete.idPaquete);
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquetes.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPaquete = new SelectList(db.ReservacionXClienteXPaquetes, "idReservacionXClienteXPaquete", "idReservacionXClienteXPaquete", paquete.idPaquete);
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPaquete,nombre,precio,lugar,disponible")] Paquete paquete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paquete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPaquete = new SelectList(db.ReservacionXClienteXPaquetes, "idReservacionXClienteXPaquete", "idReservacionXClienteXPaquete", paquete.idPaquete);
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquetes.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            return View(paquete);
        }

        public ActionResult paquete()
        {

            return View(db.Paquetes.ToList());
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paquete paquete = db.Paquetes.Find(id);
            db.Paquetes.Remove(paquete);
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
    }
}

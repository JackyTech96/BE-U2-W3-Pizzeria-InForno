using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BE_U2_W3_Pizzeria.Models;

namespace BE_U2_W3_Pizzeria.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordini
        public ActionResult Index()
        {
            var ordini = db.Ordini.Include(o => o.Utenti);
            return View(ordini.ToList());
        }

        // GET: Ordini/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // GET: Ordini/Create
        public ActionResult Create()
        {
            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username");
            return View();
        }

        // POST: Ordini/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrdine,IDUtente,DataOrdine,IsEvaso,NomeDestinatario,Indirizzo,Provincia")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Ordini.Add(ordini);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username", ordini.IDUtente);
            return View(ordini);
        }

        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username", ordini.IDUtente);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrdine,IDUtente,DataOrdine,IsEvaso,NomeDestinatario,Indirizzo,Provincia,CostoTotale")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordini).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username", ordini.IDUtente);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            return View(ordini);
        }

        // POST: Ordini/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
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

        public ActionResult OrdiniUtente()
        {
            var userId = db.Utenti.FirstOrDefault(u => u.Username == User.Identity.Name).IDUtente;
            var ordini = db.Ordini.Include(o => o.Utenti)
                .Include(o => o.DettagliOrdine)
                .Where(o => o.IDUtente == userId)
                .OrderByDescending(o => o.DataOrdine);
            return View(ordini.ToList());
        }


        public ActionResult SegnaEvaso(int id)
        {
            var ordine = db.Ordini.Find(id);
            if (ordine != null)
            {
                ordine.IsEvaso = true;
                db.SaveChanges();
                TempData["SuccessMessage"] = "L'ordine è stato segnato come evaso con successo.";
            }
            else
            {
                TempData["ErrorMessage"] = "Ordine non trovato.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult NumeroOrdiniEvasi()
        {
            int numeroOrdiniEvasi = db.Ordini.Count(o => o.IsEvaso == true);
            return Json(numeroOrdiniEvasi, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TotaleIncassato(DateTime data)
        {
            // Estrai la data senza l'ora
            DateTime dataSenzaOra = data.Date;

            // Filtra gli ordini in base alla data e allo stato "Evaso"
            decimal totaleIncassato = db.Ordini.Where(o => DbFunctions.TruncateTime(o.DataOrdine) == dataSenzaOra && o.IsEvaso)
                                               .Select(o => o.CostoTotale)
                                               .DefaultIfEmpty(0)
                                               .Sum();
            return Json(totaleIncassato, JsonRequestBehavior.AllowGet);
        }
    }
}


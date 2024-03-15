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
            // Recupera tutti gli ordini dal database, inclusi i dati degli utenti associati
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
            // Trova l'ordine corrispondente all'ID fornito
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
            // Ottiene la lista degli utenti per il menu a discesa
            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username");
            return View();
        }

        // POST: Ordini/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrdine,IDUtente,DataOrdine,IsEvaso,NomeDestinatario,Indirizzo,Provincia")] Ordini ordini)
        {
            // Verifica se il modello è valido e, in caso affermativo, aggiunge l'ordine al database
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
            // Trova l'ordine corrispondente all'ID fornito per l'editing
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUtente = new SelectList(db.Utenti, "IDUtente", "Username", ordini.IDUtente);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrdine,IDUtente,DataOrdine,IsEvaso,NomeDestinatario,Indirizzo,Provincia,CostoTotale")] Ordini ordini)
        {
            // Verifica se il modello è valido e, in caso affermativo, esegue la modifica dell'ordine nel database
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
            // Trova l'ordine corrispondente all'ID fornito per la cancellazione
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
            // Trova e rimuove l'ordine corrispondente all'ID fornito
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

        // Restituisce gli ordini associati all'utente corrente
        public ActionResult OrdiniUtente()
        {
            var userId = db.Utenti.FirstOrDefault(u => u.Username == User.Identity.Name).IDUtente;
            var ordini = db.Ordini.Include(o => o.Utenti)
                .Include(o => o.DettagliOrdine)
                .Where(o => o.IDUtente == userId)
                .OrderByDescending(o => o.DataOrdine);
            return View(ordini.ToList());
        }

        // Segna un ordine come evaso
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

        // Restituisce il numero di ordini evasi
        public ActionResult NumeroOrdiniEvasi()
        {
            int numeroOrdiniEvasi = db.Ordini.Count(o => o.IsEvaso == true);
            return Json(numeroOrdiniEvasi, JsonRequestBehavior.AllowGet);
        }

        // Restituisce il totale incassato per una determinata data
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

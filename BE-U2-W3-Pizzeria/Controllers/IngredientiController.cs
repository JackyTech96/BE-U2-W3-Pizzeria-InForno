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
    [Authorize(Roles = "admin")]
    public class IngredientiController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ingredienti
        public ActionResult Index()
        {
            return View(db.Ingredienti.ToList());
        }

        // GET: Ingredienti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredienti ingredienti = db.Ingredienti.Find(id);
            if (ingredienti == null)
            {
                return HttpNotFound();
            }
            return View(ingredienti);
        }

        // GET: Ingredienti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ingredienti/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDIngrediente,Nome")] Ingredienti ingredienti)
        {
            if (ModelState.IsValid)
            {
                db.Ingredienti.Add(ingredienti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ingredienti);
        }

        // GET: Ingredienti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredienti ingredienti = db.Ingredienti.Find(id);
            if (ingredienti == null)
            {
                return HttpNotFound();
            }
            return View(ingredienti);
        }

        // POST: Ingredienti/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDIngrediente,Nome")] Ingredienti ingredienti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingredienti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ingredienti);
        }

        // GET: Ingredienti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredienti ingredienti = db.Ingredienti.Find(id);
            if (ingredienti == null)
            {
                return HttpNotFound();
            }
            return View(ingredienti);
        }

        // POST: Ingredienti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredienti ingredienti = db.Ingredienti.Find(id);
            db.Ingredienti.Remove(ingredienti);
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

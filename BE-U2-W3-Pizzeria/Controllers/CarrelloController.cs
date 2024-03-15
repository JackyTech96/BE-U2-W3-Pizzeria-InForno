using BE_U2_W3_Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_U2_W3_Pizzeria.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Carrello
        public ActionResult Index()
        {
            // Recupera il carrello dalla sessione
            var cart = Session["cart"] as List<Prodotti>;

            // Se il carrello è vuoto o non esiste, reindirizza alla pagina dei prodotti e mostra un messaggio temporaneo
            if (cart == null || !cart.Any())
            {
                TempData["CartMessage"] = "Il carrello è vuoto";
                return RedirectToAction("Index", "Prodotti");
            }

            return View(cart);
        }

        // Azione per rimuovere un prodotto dal carrello
        public ActionResult Delete(int? id)
        {
            // Recupera il carrello dalla sessione
            var cart = Session["cart"] as List<Prodotti>;

            // Verifica se il carrello esiste e se contiene il prodotto da rimuovere
            if (cart != null)
            {
                var productToRemove = cart.FirstOrDefault(p => p.IDProdotto == id);
                if (productToRemove != null)
                {
                    cart.Remove(productToRemove);
                }
            }

            // Reindirizza alla pagina del carrello
            return RedirectToAction("Index");
        }

        // Azione per aggiungere un prodotto al carrello
        [HttpPost]
        public ActionResult AggiungiAlCarrello(int idProdotto, int quantita)
        {
            // Trova il prodotto nel database
            var productToAdd = db.Prodotti.FirstOrDefault(p => p.IDProdotto == idProdotto);
            if (productToAdd != null)
            {
                // Recupera il carrello dalla sessione o crea un nuovo carrello se non esiste
                var cart = Session["cart"] as List<Prodotti>;
                if (cart == null)
                {
                    cart = new List<Prodotti>();
                    Session["cart"] = cart;
                }

                // Controlla se il prodotto è già presente nel carrello
                var existingProduct = cart.FirstOrDefault(p => p.IDProdotto == idProdotto);
                if (existingProduct != null)
                {
                    // Se il prodotto è già presente, aggiorna la quantità
                    existingProduct.Quantita += quantita;
                }
                else
                {
                    // Altrimenti, aggiungi il prodotto al carrello con la quantità specificata
                    productToAdd.Quantita = quantita;
                    cart.Add(productToAdd);
                }
            }

            // Mostra un messaggio temporaneo e reindirizza alla pagina dei prodotti
            TempData["CartMessage"] = "Prodotto aggiunto al carrello";
            return RedirectToAction("Index", "Prodotti");
        }

        // Azione per svuotare il carrello
        public ActionResult SvuotaCarrello()
        {
            // Rimuove il carrello dalla sessione
            Session["cart"] = null;

            // Mostra un messaggio temporaneo e reindirizza alla pagina dei prodotti
            TempData["CartMessage"] = "Il carrello è stato svuotato";
            return RedirectToAction("Index", "Prodotti");
        }

        [HttpPost]
        public ActionResult Ordina(string nomeDestinatario, string indirizzo, string provincia)
        {
            // Controlla se l'utente è autenticato e recupera il suo ID
            var userId = db.Utenti.FirstOrDefault(u => u.Username == User.Identity.Name)?.IDUtente;

            // Recupera il carrello dalla sessione
            var cart = Session["cart"] as List<Prodotti>;
            if (userId != null && cart != null && cart.Any())
            {
                // Crea un nuovo ordine
                Ordini newOrder = new Ordini();
                newOrder.DataOrdine = DateTime.Now;
                newOrder.IsEvaso = false;
                newOrder.IDUtente = userId.Value; // Utilizza IDUtente anziché FK_idUtente
                newOrder.NomeDestinatario = nomeDestinatario;
                newOrder.Indirizzo = indirizzo;
                newOrder.Provincia = provincia;

                // Aggiungi l'ordine al database
                db.Ordini.Add(newOrder);
                db.SaveChanges();

                // Aggiungi i dettagli dell'ordine per ciascun prodotto nel carrello
                foreach (var product in cart)
                {
                    DettagliOrdine newDetail = new DettagliOrdine();
                    newDetail.IDOrdine = newOrder.IDOrdine;
                    newDetail.IDProdotto = product.IDProdotto;
                    newDetail.Quantita = Convert.ToInt32(product.Quantita);

                    // Calcola il prezzo totale del singolo prodotto e aggiungilo al costo totale dell'ordine
                    decimal prezzoProdotto = product.PrezzoTotale;
                    newOrder.CostoTotale += prezzoProdotto;

                    // Aggiungi il dettaglio dell'ordine al database
                    db.DettagliOrdine.Add(newDetail);
                    db.SaveChanges();
                }

                // Svuota il carrello
                cart.Clear();

                // Reindirizza alla vista di conferma dell'ordine
                return RedirectToAction("ConfermaOrdine");
            }

            // Nel caso in cui l'utente non sia autenticato o il carrello sia vuoto, gestisci l'errore appropriato
            TempData["ErrorMessage"] = "Errore nell'ordinazione. Assicurati di essere autenticato e di avere elementi nel carrello.";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfermaOrdine()
        {
            ViewBag.TempoAttesa = 30; // Tempo di attesa stimato
            return View();
        }
    }
}

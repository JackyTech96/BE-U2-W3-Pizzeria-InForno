namespace BE_U2_W3_Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prodotti")]
    public partial class Prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotti()
        {
            DettagliOrdine = new HashSet<DettagliOrdine>();
        }

        [Key]
        public int IDProdotto { get; set; }

        public int IDIngrediente { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Nome del Prodotto")] // Modifica qui
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Immagine del Prodotto")] // Modifica qui
        public string Immagine { get; set; }

        [Display(Name = "Prezzo Totale")] // Modifica qui
        public decimal PrezzoTotale { get; set; }

        [Display(Name = "Tempo di Consegna(minuti)")] // Modifica qui
        public int TempoConsegna { get; set; } = 30;

        [Display(Name = "Quantità")]
        public int Quantita { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettagliOrdine> DettagliOrdine { get; set; }

        public virtual Ingredienti Ingredienti { get; set; }
    }
}

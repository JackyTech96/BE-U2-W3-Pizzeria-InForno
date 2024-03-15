namespace BE_U2_W3_Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DettagliOrdine")]
    public partial class DettagliOrdine
    {
        [Key]
        public int IDDettaglioOrdine { get; set; }

        public int IDOrdine { get; set; }

        public int IDProdotto { get; set; }

        public int Quantita { get; set; }

        public string Note { get; set; }

        public virtual Ordini Ordini { get; set; }

        public virtual Prodotti Prodotti { get; set; }
    }
}

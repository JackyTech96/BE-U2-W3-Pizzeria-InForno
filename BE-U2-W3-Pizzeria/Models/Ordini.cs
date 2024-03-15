namespace BE_U2_W3_Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            DettagliOrdine = new HashSet<DettagliOrdine>();
        }

        [Key]
        public int IDOrdine { get; set; }

        public int IDUtente { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOrdine { get; set; }

        public bool IsEvaso { get; set; }

        [Required]
        [StringLength(50)]
        public string NomeDestinatario { get; set; }

        [Required]
        public string Indirizzo { get; set; }

        [Required]
        [StringLength(2)]
        public string Provincia { get; set; }

        public decimal CostoTotale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettagliOrdine> DettagliOrdine { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}

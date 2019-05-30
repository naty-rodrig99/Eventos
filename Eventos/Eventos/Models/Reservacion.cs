//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eventos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservacion()
        {
            this.ClienteXReservacions = new HashSet<ClienteXReservacion>();
            this.ReservacionXRecursoes = new HashSet<ReservacionXRecurso>();
        }
    
        public int idReservacion { get; set; }
        public string numReservacion { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<System.TimeSpan> hora { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> idPaquete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClienteXReservacion> ClienteXReservacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservacionXRecurso> ReservacionXRecursoes { get; set; }
    }
}

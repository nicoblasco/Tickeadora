//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TickeadoraTurnos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CallCenterTurns
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CallCenterTurns()
        {
            this.Turns = new HashSet<Turns>();
        }
    
        public int Id { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoTramite { get; set; }
        public System.DateTime FechaTurno { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool Asignado { get; set; }
        public string Gestion { get; set; }
        public string Tel_Particular { get; set; }
        public string Tel_Celular { get; set; }
        public string Estado { get; set; }
        public string Barrio { get; set; }
        public string Vencimiento_licencia { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> UsuarioId { get; set; }
    
        public virtual Usuarios Usuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Turns> Turns { get; set; }
    }
}

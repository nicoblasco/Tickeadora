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
    
    public partial class Workflows
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Workflows()
        {
            this.SectorWorkflows = new HashSet<SectorWorkflows>();
        }
    
        public int Id { get; set; }
        public int TypesLicenseID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SectorWorkflows> SectorWorkflows { get; set; }
        public virtual TypesLicenses TypesLicenses { get; set; }
    }
}

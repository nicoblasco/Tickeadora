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
    
    public partial class Nighborhoods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nighborhoods()
        {
            this.People = new HashSet<People>();
        }
    
        public int Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Direccion { get; set; }
        public string Altura { get; set; }
        public string Partida { get; set; }
        public string Barrio { get; set; }
        public string Nombre { get; set; }
        public string Zona { get; set; }
        public string NombreCorto { get; set; }
        public int Codigo { get; set; }
        public string Habitantes2010 { get; set; }
        public string ProyecionHabitantes2016 { get; set; }
        public string ProyecionHabitantes2020 { get; set; }
        public string Detalle { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Perimetro { get; set; }
        public string Area { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<People> People { get; set; }
    }
}

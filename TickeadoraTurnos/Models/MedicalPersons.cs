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
    
    public partial class MedicalPersons
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int TurnId { get; set; }
        public int Avoi { get; set; }
        public int Avod { get; set; }
        public bool Fuma { get; set; }
        public bool Profesional { get; set; }
        public bool ConduceConAnteojos { get; set; }
        public bool VisionMonocular { get; set; }
        public bool Discromatopsia { get; set; }
        public bool HTA { get; set; }
        public bool DBT { get; set; }
        public bool GAA { get; set; }
        public bool AcidoUrico { get; set; }
        public bool Colesterol { get; set; }
        public string Observacion { get; set; }
    
        public virtual People People { get; set; }
        public virtual Turns Turns { get; set; }
    }
}
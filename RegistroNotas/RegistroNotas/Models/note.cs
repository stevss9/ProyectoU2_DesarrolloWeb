//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistroNotas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class note
    {
        public int id { get; set; }
        public Nullable<decimal> note_1 { get; set; }
        public Nullable<decimal> note_2 { get; set; }
        public Nullable<decimal> note_3 { get; set; }
        public Nullable<decimal> note_4 { get; set; }
        public Nullable<int> id_student { get; set; }
        public Nullable<int> id_course { get; set; }
    
        public virtual cours cours { get; set; }
        public virtual estudent estudent { get; set; }
    }
}

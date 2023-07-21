using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroNotas.Models.ViewModels
{
    public class NoteListView
    {
        public int id { get; set; }
        public decimal? note_1 { get; set; }
        public decimal? note_2 { get; set; }
        public decimal? note_3 { get; set; }
        public decimal? note_4 { get; set; }
        public int? id_student { get; set; }
        public int? id_course { get; set; }
    }
}
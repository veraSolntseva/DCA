using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DbObjects
{
    public abstract class SheetItem
    {
        [Key]
        public int ID { get; set; }

        public string Col1 { get; set; }

        public string Col2 { get; set; }

        public string Col3 { get; set; }

        public string Col4 { get; set; }

        public string Col5 { get; set; }

        public string Col6 { get; set; }

        public string Col7 { get; set; }

        public string Col8 { get; set; }

        public string Col9 { get; set; }

        public string Col10 { get; set; }

        public string Col11 { get; set; }

        public string Col12 { get; set; }

        public string Col13 { get; set; }

        public string Col14 { get; set; }

        public string Col15 { get; set; }

        public string Col16 { get; set; }

        public string Col17 { get; set; }

        public string Col18 { get; set; }

        public string Col19 { get; set; }

        public string Col20 { get; set; }

        public DateTime DtAdd { get; set; }

        public DateTime? DtEdit { get; set; }

        public DateTime? DtDelete { get; set; }
    }
}

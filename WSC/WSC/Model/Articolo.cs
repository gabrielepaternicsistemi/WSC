using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSC.Model
{
    public class Articolo
    {
        public string ar_descr { get; set; }
        public int ar_codiva { get; set; }
        public int ar_gruppo { get; set; }
        public int ar_sotgru { get; set; }
        public string ar_codart { get; set; }
        public string bc_code { get; set; }
        public decimal lc_listino1 { get; set; }
        public string ar_unmis { get; set; }
    }
}
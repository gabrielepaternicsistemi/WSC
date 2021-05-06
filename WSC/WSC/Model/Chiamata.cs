using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSC.Model
{
   

    public class ChiamataMobile
    {
        public List<Attivita> attivita { get; set; }
        public Chiamata chiamata { get; set; }
    }

    public class Chiamata
    {
        public string op_CODART { get; set; }
        public int op_CODCHIA { get; set; }
        public int op_CODLEAD { get; set; }
        public int op_CODTCHI { get; set; }
        public int op_CONTO { get; set; }
        public long op_DATCHIA { get; set; }
        public string op_MATRIC1 { get; set; }
        public string op_NOTE { get; set; }
        public int op_NUMCONTR { get; set; }
        public string op_OGGETTO { get; set; }
        public int op_OPINC { get; set; }
        public string op_STATUS { get; set; }
    }

    public class Attivita
    {
        public string ac_CHIAVEGLOBALE { get; set; }
        public string ac_CODART { get; set; }
        public string ac_CODARTVIA { get; set; }
        public int ac_CODATTC { get; set; }
        public int ac_CODCHIA { get; set; }
        public int ac_CODLEAD { get; set; }
        public int ac_CODTACO { get; set; }
        public int ac_CODTCHI { get; set; }
        public int ac_CONTO { get; set; }
        public long ac_DATAESEC { get; set; }
        public Byte[] ac_FRIMA { get; set; }
        public string ac_HHMATRIC1 { get; set; }
        public string ac_NOTE { get; set; }
        public string ac_OGGETTO { get; set; }
        public int ac_OPINC { get; set; }
        public float ac_ORAESEC { get; set; }
        public float ac_ORAFINE { get; set; }
        public float ac_ORAINIZIO { get; set; }
        public float ac_PREZZO { get; set; }
        public float ac_PREZZOVIA { get; set; }
        public float ac_QUANT { get; set; }
        public float ac_QUANTVIA { get; set; }
        public string ac_STATUSFATT { get; set; }
        public string ac_TIPOADDE { get; set; }
        public string ac_TIPOADDEVIA { get; set; }
        public float ac_VALORE { get; set; }
        public float ac_VALOREVIA { get; set; }
    }

}
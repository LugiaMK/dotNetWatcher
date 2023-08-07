using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaOneA.Model
{
    public class Model
    {
        [Key]
        public int Notional { get; set; }
        public string TradeID { get; set; }
        public string ISIN { get; set; }
    }
}

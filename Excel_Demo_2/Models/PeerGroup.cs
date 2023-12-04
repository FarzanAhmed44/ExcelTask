using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Excel_Demo_2.Models
{
    public class PeerGroup
    {
        public int Id { get; set; }
        public string Particular { get; set; }
        public string Unit { get; set; }
  
        public string Empty { get; set; }

       //  public string IndustryQuartile { get; set; }

        public byte[] IndustryQuartile { get; set; }


        public string PeerGroupAverage { get; set; }

   
        public string PeerGroupMedian { get; set; }


  
        public string PeerGroupMin { get; set; }

       
        public string PeerGroupMax { get; set; }
    }
}

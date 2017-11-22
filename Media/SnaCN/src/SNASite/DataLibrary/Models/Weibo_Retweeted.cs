using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    public partial class Weibo_Retweeted
    {
        public string id_to { get; set; }
        public string id_from { get; set; }
        public Nullable<int> weight { get; set; }
        public string to_color { get; set; }
        public string from_color { get; set; }
        public Nullable<int> from_weight { get; set; }
        public Nullable<int> to_weight { get; set; }

        [NotMapped]
        public string mid { get; set; }
    }
}

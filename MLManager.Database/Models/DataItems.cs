using System;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("data_item")]
    public class DataItem
    {
        public long DataItemId { get; set; }
        public long DatasetId { get; set; }
        public long UserId { get; set; }

        [Column(TypeName = "jsonb")]
        public string LabelJson { get; set; }
        public DateTime CreationTimestamp { get; set;}
    }
}
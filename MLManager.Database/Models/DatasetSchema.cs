using System;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("dataset_schema")]
    public class DatasetSchema
    {
        public long DatasetId { get; set; }
        public int VersionId { get; set; }
        public long UserId { get; set; }

        [Column(TypeName = "jsonb")]
        public string Schema { get; set; }
        public DateTime CreationTimestamp { get; set; }
    }
}
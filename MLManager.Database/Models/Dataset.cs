using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("datasets")]
    public class Dataset
    {
        [Key]
        public long DatasetId { get; set; }

        [Required]
        [MaxLength(128)]
        public string DatasetName { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime CreationTimestamp { get; set; }
    }
}
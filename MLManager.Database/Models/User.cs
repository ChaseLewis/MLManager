using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MLManager.Database
{
    [Table("users")]
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [Required]
        [MinLength(1)]
        [Column(TypeName = "TEXT")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [Column(TypeName = "TEXT")]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        [Column(TypeName = "TEXT")]
        public string Username { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(60)]
        [Column(TypeName = "CHAR(60)")]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(TypeName = "TEXT")]
        public string Email { get; set; }

        [MaxLength(12)]
        [Column(TypeName = "CHAR(12)")]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegistrationTimestamp { get; set; }

        public DateTime? VerifiedEmailTimestamp { get; set; }
    }
}
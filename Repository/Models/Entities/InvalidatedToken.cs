using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Entities
{
    [Table("InvalidatedToken")]
    public class InvalidatedToken
    {
        [Key]
        public string Id { get; set; }

        public DateTime ExpiryTime { get; set; }
    }
}

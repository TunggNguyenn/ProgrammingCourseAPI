using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiryOn { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        //[Required]
        public DateTime? RevokedOn { get; set; }

        [Required] //
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

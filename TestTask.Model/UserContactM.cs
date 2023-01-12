using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Model
{
    public class UserContactM : BaseEntity
    {
        [Key]
        public Guid UserConId { get; set; }

        [Display(Name = "Email ID")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailID { get; set; } = null!;

        [Display(Name = "Mobile Number")]
        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Mobile is required.")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number. Mobile number must contain 10 numbers")]
        public string Mobile { get; set; } = null!;

        public Guid UserId { get; set; }
        public virtual UserM? user { get; set; }

    }
}

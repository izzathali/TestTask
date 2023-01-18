using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestTask.Model
{
    public class UserM : BaseEntity
    {
        [Key]
        public Guid UserId { get; set; }


        [Display(Name = "Full Name")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[@$!%*?&])([a-zA-Z0-9@$!%*?&]{8,})$", ErrorMessage = "Password must contain at least 1 special character and have a minimum length of 8 letters.")]
        public string Password { get; set; } = null!;

        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; } = string.Empty;
        [NotMapped]
        [Required(ErrorMessage = "Photo is required.")]
        public IFormFile? iPhotoFile { get; set; }

        public virtual List<UserContactM>? userContacts { get; set; } = new List<UserContactM>();
    }
}

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
        public string FullName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public int Gender { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; } = null!;
        [NotMapped]
        public IFormFile? iPhotoFile { get; set; }

        public virtual List<UserContactM>? userContacts { get; set; }
    }
}

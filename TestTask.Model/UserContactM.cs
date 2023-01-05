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
        public int UserContactId { get; set; }

        [Display(Name = "Email ID")]
        [Column(TypeName = "nvarchar(250)")]
        public string? EmailID { get; set; }

        [Display(Name = "Mobile Number")]
        [Column(TypeName = "nvarchar(10)")]
        public string? Mobile { get; set; }

        public Guid UserId { get; set; }
        public virtual UserM? user { get; set; }

    }
}

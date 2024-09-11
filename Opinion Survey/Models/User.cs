using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Opinion_Survey.Models
{
    public enum Gender
    {
        Male=1,Female
    }
    public class User:IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public Gender Gender { get; set; }

     
        //[Required]
        //public int Age { get; set; }
        //[Required]
        //public Gender Gender { get; set; }
        //public int? Points { get; set; }
        //public int NumOfSurveys { get; set; }


    }
}

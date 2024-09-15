using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    
        public string? Imagepath { get; set; } 
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
     
        //[Required]
        //public int Age { get; set; }
        //[Required]
        //public Gender Gender { get; set; }
        //public int? Points { get; set; }
        //public int NumOfSurveys { get; set; }


    }
}

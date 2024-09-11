using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Opinion_Survey.Models;
using Type = Opinion_Survey.Models.Type;

namespace Opinion_Survey.DTO
{
    public class ModifiedUserDTO
    {
        [PersonalData]
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? FirstName { get; set; }

        [PersonalData]
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
       
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


    }
    public class LogIn
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class FormModified
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public string OwnerName { get; set; }
        //public string WebTitle { get; set; }
        //public int Points { get; set; }
        //public int MaxResponsses { get; set; }
        public string QuestionText { get; set; }
        //public string OptionText { get; set; }

        public Type Type { get; set; }


    }
}

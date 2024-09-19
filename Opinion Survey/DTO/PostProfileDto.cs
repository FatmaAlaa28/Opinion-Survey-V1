using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Opinion_Survey.DTO
{
    public class PostProfileDto
    {
        [PersonalData]

        [Column(TypeName = "nvarchar(50)")]
        public string? FirstName { get; set; }

        [PersonalData]

        [Column(TypeName = "nvarchar(50)")]
        public string? LastName { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}

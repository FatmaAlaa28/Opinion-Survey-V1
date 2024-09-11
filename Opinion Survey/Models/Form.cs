using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Opinion_Survey.Models
{
    public enum State
    {
        Open,
        Closed
    }

    public class Form
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; }

        [ForeignKey("Folders")]
        public int? FolderID { get; set; }

        public virtual User Users { get; set; }
        public virtual Folder Folders { get; set; }
        public virtual ICollection<Question> Questions { get; set; } // Navigation property
    }
}

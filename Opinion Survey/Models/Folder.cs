using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Opinion_Survey.Models
{
    public class Folder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Nullable parent folder
        [ForeignKey("ParentFolder")]
        public int? ParentFolderId { get; set; }

        // Self-referencing relationship
        public virtual Folder ParentFolder { get; set; }

        // If you want a list of child folders for this folder
        public virtual ICollection<Folder> ChildFolders { get; set; } = new List<Folder>();
    }
}

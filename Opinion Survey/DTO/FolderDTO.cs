using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Opinion_Survey.Models;
namespace Opinion_Survey.DTO
{
    public class FolderDTO
    {
        public int FolderId { get; set; }

        public string FolderName { get; set; }
        public int? ParentFolderId { get; set; }

    }
}

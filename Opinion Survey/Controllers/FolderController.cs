using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opinion_Survey.DTO;
using Opinion_Survey.Models;
using Opinion_Survey.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Opinion_Survey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {

        private AppDbContext _context;
        public FolderController(AppDbContext context)
        {
            _context = context;
        }

        //folder is parent and have child(Folder,Forms)
        [HttpGet("GetFolder/{idFolder}")]
        public IActionResult GetElementOfFolder([FromRoute] int idFolder)
        {
            List<Folder> folders = _context.Folders.Where(x => x.ParentFolderId == idFolder).ToList();
            List<Form> forms = _context.Forms.Where(x=> x.FolderID == idFolder).ToList();
            
            
            return Ok(new {lst1=folders,lst2=forms});
        }


        [HttpPost("NewFolder")]
        public IActionResult CreateFolder(string FolderName , int? ParentFolderID) {
        Folder folder = new Folder();
            folder.Name = FolderName;
            if(ParentFolderID != null)
                folder.ParentFolderId = ParentFolderID;
            _context.Folders.Add(folder);
            _context.SaveChanges();
            return Ok();
        }
    }
}

namespace Opinion_Survey.DTO
{
    public class FormDTO
    {

        public int FormId { get; set; }

        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public string UserIdForForm { get; set; }

        public int? FolderID { get; set; }


    }
}

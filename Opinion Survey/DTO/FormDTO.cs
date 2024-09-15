using Opinion_Survey.Models;

namespace Opinion_Survey.DTO
{
    public class FormDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
      
        public List<QuestionDto> Questions { get; set; }
        //public string FormTitle { get; set; }
        //public string FormDescription { get; set; }
        //public string UserIdForForm { get; set; }

        //public List<Question> questions { get; set; }

        ////public string QuestionText { get; set; }
        ////public Type Type { get; set; }
        ////public string OptionText { get; set; }

        //public int? FolderID { get; set; }


    }
}

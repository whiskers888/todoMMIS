namespace todoMMIS.Models
{
    public class EFTodoItem : EFBaseModel
    {

        public string TaskDescription { get; set; }
        public string Author { get; set; }
        public bool IsComplete { get; set; }
    }
}


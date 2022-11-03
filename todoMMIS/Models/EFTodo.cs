namespace todoMMIS.Models
{
    public class EFTodo : EFBaseModel
    {

        public string TaskDescription { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public bool IsComplete { get; set; }
    }
}


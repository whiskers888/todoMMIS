using System;

namespace todoMMIS.Models
{
    public class EFTodo : EFBaseModel
    {

        public string TaskDescription { get; set; }
        public string User { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsExpired { get; set;}
        public DateTime? createdAt { get; set; }
        public DateTime? expiredAt { get; set; }
        public int Priority { get; set; }

    }
}


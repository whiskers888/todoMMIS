﻿namespace todoMMIS.Models
{
    public class EFTodo : EFBaseModel
    {

        public string TaskDescription { get; set; }
        public string User { get; set; }
        public bool IsComplete { get; set; }
        /*public DateTime DateCreate { get; set; }
        public DateTime DateExpired { get; set; }*/

    }
}


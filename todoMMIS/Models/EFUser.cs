﻿namespace todoMMIS.Models
{
    public class EFUser : EFBaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Token { get; set; }
    }
}

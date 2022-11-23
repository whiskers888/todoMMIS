namespace todoMMIS.Models.EF
{
    public class EFUser : EFBaseModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}

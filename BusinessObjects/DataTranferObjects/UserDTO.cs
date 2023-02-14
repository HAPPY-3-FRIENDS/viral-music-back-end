namespace BusinessObjects.DataTranferObjects
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public int? RoleId { get; set; }
    }
}
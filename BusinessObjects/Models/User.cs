using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class User
    {
        public User()
        {
            Playlists = new HashSet<Playlist>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
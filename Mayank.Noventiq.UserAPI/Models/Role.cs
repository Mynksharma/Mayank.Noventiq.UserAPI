using System.ComponentModel.DataAnnotations;

namespace Mayank.Noventiq.UserAPI.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

using CareerApplication.Core.Entities;

namespace CareerApplication.Admin.Models
{
    public class UserModel : UserEntity
    {
        public string Password { get; set; } = string.Empty;
        public RoleModel Role { get; set; }
    }
}

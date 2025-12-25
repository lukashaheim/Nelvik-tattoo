using System.Collections.Generic;

namespace Nelvik_Tattoo.Models.Admin
{
    public class UsersAdminIndexVm
    {
        public CreateStaffUserVm Create { get; set; } = new();
        public List<UserRowVm> Users { get; set; } = new();
        public string? StatusMessage { get; set; }
    }

    public class UserRowVm
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsLockedOut { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
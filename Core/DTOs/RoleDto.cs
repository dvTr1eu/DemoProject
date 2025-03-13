using Microsoft.AspNetCore.Identity;

namespace Core.DTOs
{
    public class RoleDto
    {
        public string RoleName { get; set; }

        public static RoleDto MapFromRole(IdentityRole role)
        {
            return new RoleDto()
            {
                RoleName = role.Name
            };
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CBIB.Configuration
{
    public class UserRoleSeed
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleSeed(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async void Seed()
        {
            if( (await _roleManager.FindByNameAsync("Global Administrator")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Global Administrator" });

            }

            if ((await _roleManager.FindByNameAsync("Node Administrator")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Node Administrator" });

            }

            if ((await _roleManager.FindByNameAsync("Member")) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

            }
        }
    }
}

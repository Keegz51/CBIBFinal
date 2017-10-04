using CBIB.Data;
using CBIB.Models;
using CBIB.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CBIB.Controllers
{
    //[Authorize(Roles="Global Administrator")]

    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CBIBContext _CBIBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(
            ApplicationDbContext dbContext,
            CBIBContext CBIBContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _CBIBContext = CBIBContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Author> authorList = null;
            var vm = new UserManagementIndexViewModel();
            List<ApplicationUser> UserList = new List<ApplicationUser>();

            var user = await _userManager.GetUserAsync(User);
            long currentUserNode = (await _CBIBContext.Author.FindAsync(user.AuthorID)).NodeID;

            if (User.IsInRole("Global Administrator"))
            {
                vm.Users = _dbContext.Users.OrderBy(u => u.Email).Include(u => u.Roles).ToList();
            }

            else
            {
                authorList = _CBIBContext.Author.ToList();
                List<Author> list = null;
                List<ApplicationUser> Users = null;

                foreach (Author a in authorList)
                {
                    if (a.NodeID == currentUserNode)
                    {
                        list.Add(a);
                    }
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole(string id)
        {
            var user = await GetUserById(id);
            var vm = new UserManagementAddRoleViewModel
            {
                Roles = GetAllRoles(),
                UserId = id,
                Email = user.Email
            };
            return View(vm);
        }
       
        [HttpPost]
        public async Task<IActionResult> AddRole(UserManagementAddRoleViewModel rvm)
        {
            var user = await GetUserById(rvm.UserId);
            if (ModelState.IsValid)
            {
                var result = await _userManager.AddToRoleAsync(user, rvm.NewRole);
            
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            rvm.Email = user.Email;
            rvm.Roles = GetAllRoles();
            return View(rvm);
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
            await _userManager.FindByIdAsync(id);

        private SelectList GetAllRoles() => new SelectList(_roleManager.Roles.OrderBy(r => r.Name));
    }
}

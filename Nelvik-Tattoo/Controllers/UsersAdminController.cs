using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nelvik_Tattoo.Data;
using Nelvik_Tattoo.Models.Admin;

namespace Nelvik_Tattoo.Controllers
{
    [Authorize(Policy = "OwnerOnly")]
    [Route("theking/users")]
    public class UsersAdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersAdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET /theking/users
        [HttpGet("")]
        public async Task<IActionResult> Index(string? status = null)
        {
            var users = _userManager.Users
                .OrderBy(u => u.Email)
                .ToList();

            var vm = new UsersAdminIndexVm { StatusMessage = status };

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var lockedOut = await _userManager.IsLockedOutAsync(u);

                vm.Users.Add(new UserRowVm
                {
                    Id = u.Id,
                    Email = u.Email ?? u.UserName ?? "",
                    EmailConfirmed = u.EmailConfirmed,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    IsLockedOut = lockedOut,
                    Roles = roles
                });
            }

            return View(vm);
        }

        // POST /theking/users/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "Create")] CreateStaffUserVm input)
        {
            if (!ModelState.IsValid)
            {
                var vm = await BuildIndexVmAsync();
                vm.Create = input;
                return View("Index", vm);
            }

            var email = input.Email.Trim().ToLowerInvariant();

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(input.Email), "Denne e-posten er allerede i bruk.");
                var vm = await BuildIndexVmAsync();
                vm.Create = input;
                return View("Index", vm);
            }

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                var vm = await BuildIndexVmAsync();
                vm.Create = input;
                return View("Index", vm);
            }

            // Kun for oversikt/merking (ikke for tilgang i admin)
            await _userManager.AddToRoleAsync(user, OwnerUserSeeder.StaffRole);

            return RedirectToAction(nameof(Index), new { status = $"Bruker '{email}' ble opprettet." });
        }
        
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            // Ikke tillat å slette seg selv
            var currentUserId = _userManager.GetUserId(User);
            if (id == currentUserId)
                return RedirectToAction(nameof(Index), new { status = "Du kan ikke slette din egen bruker." });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction(nameof(Index), new { status = "Bruker finnes ikke." });

            // Beskytt owner-brukere mot sletting (valgfritt men anbefalt)
            if (await _userManager.IsInRoleAsync(user, OwnerUserSeeder.OwnerRole))
                return RedirectToAction(nameof(Index), new { status = "Owner-bruker kan ikke slettes." });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var msg = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index), new { status = $"Kunne ikke slette bruker: {msg}" });
            }

            return RedirectToAction(nameof(Index), new { status = $"Bruker '{user.Email}' ble slettet." });
        }


        private async Task<UsersAdminIndexVm> BuildIndexVmAsync()
        {
            var users = _userManager.Users
                .OrderBy(u => u.Email)
                .ToList();

            var vm = new UsersAdminIndexVm();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var lockedOut = await _userManager.IsLockedOutAsync(u);

                vm.Users.Add(new UserRowVm
                {
                    Id = u.Id,
                    Email = u.Email ?? u.UserName ?? "",
                    EmailConfirmed = u.EmailConfirmed,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    IsLockedOut = lockedOut,
                    Roles = roles
                });
            }

            return vm;
        }
    }
}

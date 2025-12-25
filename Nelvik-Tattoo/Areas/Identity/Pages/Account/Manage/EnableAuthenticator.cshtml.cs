using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Nelvik_Tattoo.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class EnableAuthenticatorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EnableAuthenticatorModel> _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat =
            "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public EnableAuthenticatorModel(
            UserManager<IdentityUser> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        public string SharedKey { get; set; } = string.Empty;
        public string AuthenticatorUri { get; set; } = string.Empty;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [StringLength(7, MinimumLength = 6, ErrorMessage = "Verification code must be at least {2} and at most {1} characters long.")]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Unable to load user.");

            await LoadSharedKeyAndQrCodeUriAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Unable to load user.");

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID '{UserId}' has enabled 2FA.", await _userManager.GetUserIdAsync(user));

            StatusMessage = "Your authenticator app has been verified.";
            return RedirectToPage("./TwoFactorAuthentication");
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = FormatKey(unformattedKey!);
            AuthenticatorUri = GenerateQrCodeUri(user.Email ?? user.UserName ?? "user", unformattedKey!);
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            var issuer = _urlEncoder.Encode("Nelvik Tattoo");
            return string.Format(
                AuthenticatorUriFormat,
                issuer,
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            for (int i = 0; i + 4 < unformattedKey.Length; i += 4)
            {
                result.Append(unformattedKey.Substring(i, 4)).Append(' ');
            }

            if (unformattedKey.Length % 4 != 0)
                result.Append(unformattedKey.Substring(unformattedKey.Length - unformattedKey.Length % 4));

            return result.ToString().ToLowerInvariant();
        }
    }
}

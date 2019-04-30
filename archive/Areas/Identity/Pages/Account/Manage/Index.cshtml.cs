using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace archive.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<archive.Data.Entities.ApplicationUser> _userManager;
        private readonly SignInManager<archive.Data.Entities.ApplicationUser> _signInManager;
        private readonly IRepository _repository;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<archive.Data.Entities.ApplicationUser> userManager,
            SignInManager<archive.Data.Entities.ApplicationUser> signInManager,
            IEmailSender emailSender, IRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _repository = repository;
        }

        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [Display(Name = "Awatar")]
        public byte[] AvatarImage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Adres email")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Strona domowa")]
            public string HomePage { get; set; }

            [Display(Name = "Ustaw nowy awatar")]
            public IFormFile AvatarImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var id = await _userManager.GetUserIdAsync(user);

            Username = userName;
            AvatarImage = (await _repository.Users
                    .Include(u => u.Avatar)
                    .FirstOrDefaultAsync(a => a.Id == id))?
                .Avatar?.Image;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                HomePage = user.HomePage,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            user.HomePage = Input.HomePage;
            if (!(await _userManager.UpdateAsync(user)).Succeeded)
            {
                throw new InvalidOperationException(
                    $"Unexpected error occurred setting homepage for user with ID '{userId}'.");
            }

            if (Input.AvatarImage != null)
            {
                var stream = Input.AvatarImage.OpenReadStream();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                user.Avatar = new UserAvatar {Image = memoryStream.ToArray(), ApplicationUserId = user.Id};
                if (!(await _userManager.UpdateAsync(user)).Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting avatar for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new {userId = userId, code = code},
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(email, "Potwierdzenie adresu email",
                $"Potwierdź swój adres email <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");

            StatusMessage = "Email weryfikacyjny został wysłany. Proszę o sprawdzenie skrzynki mailowej.";
            return RedirectToPage();
        }
    }
}
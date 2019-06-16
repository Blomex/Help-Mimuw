using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using archive.Data;
using archive.Data.Entities;
using archive.Services.Users;
using Microsoft.AspNetCore.Http;
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
        private readonly IAchievementsService _achievementsService;
        public IndexModel(
            UserManager<archive.Data.Entities.ApplicationUser> userManager,
            SignInManager<archive.Data.Entities.ApplicationUser> signInManager,
            IEmailSender emailSender, IRepository repository,
            IAchievementsService achievementsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _repository = repository;
            _achievementsService = achievementsService;
        }

        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [Display(Name = "Awatar")]
        public byte[] AvatarImage { get; set; }
        [Display(Name = "Achievementy")]
        public ICollection<Achievement> UserAchievements { get; set; }
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
            UserAchievements = await _achievementsService.UsersAchievements(user);
            Username = user.UserName;
            AvatarImage = (await _repository.Users
                    .Include(u => u.Avatar)
                    .FirstOrDefaultAsync(a => a.Id == user.Id))?
                .Avatar?.Image;

            Input = new InputModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HomePage = user.HomePage
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

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            user.HomePage = Input.HomePage;
            if (!(await _userManager.UpdateAsync(user)).Succeeded)
            {
                throw new InvalidOperationException(
                    $"Unexpected error occurred setting homepage for user with ID '{user.Id}'.");
            }

            if (Input.AvatarImage != null)
            {
                var stream = Input.AvatarImage.OpenReadStream();
                if (stream.Length > UserAvatar.ImageSizeLimit)
                {
                    StatusMessage = $"Błąd. Przekroczono limit {UserAvatar.ImageSizeLimit} bajtów per awatar.";
                    return RedirectToPage();
                }

                if (!Input.AvatarImage.ContentType.Contains("image"))
                {
                    StatusMessage = $"Błąd. Awatar nie jest obrazkiem.";
                    return RedirectToPage();
                }
                
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                var avatar = await _repository.Avatars
                    .FirstOrDefaultAsync(a => a.ApplicationUserId == user.Id);

                if (avatar != null)
                {
                    avatar.Image = memoryStream.ToArray();
                    await _repository.SaveChangesAsync();
                }
                else
                {
                    avatar = new UserAvatar {Image = memoryStream.ToArray(), ApplicationUserId = user.Id};
                    user.Avatar = user.Avatar;
                    _repository.Avatars.Add(avatar);
                    await _repository.SaveChangesAsync();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został pomyślnie zaktualizowany.";
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
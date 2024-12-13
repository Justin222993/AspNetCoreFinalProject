using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.ViewModels;

public class AccountController : Controller
{
    private readonly GalleryContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        GalleryContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.Role == "Artist")
                {
                    if (string.IsNullOrWhiteSpace(model.Bio) || string.IsNullOrWhiteSpace(model.ProfileImageUrl))
                    {
                        ModelState.AddModelError(string.Empty, "Bio and Profile Image URL are required for Artists.");
                        return View(model);
                    }

                    await _userManager.AddToRoleAsync(user, "Artist");

                    var artist = new Artist
                    {
                        FullName = model.FullName,
                        Bio = model.Bio,
                        ProfileImageUrl = model.ProfileImageUrl,
                        UserId = user.Id,
                        Artworks = new List<Artwork>()
                    };

                    // Add artist to the database
                    _context.Artists.Add(artist);
                    await _context.SaveChangesAsync();
                }
                else if (model.Role == "Buyer")
                {
                    await _userManager.AddToRoleAsync(user, "Buyer");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role selected.");
                    return View(model);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }







    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var role = userRoles.FirstOrDefault();

                    if (role != null)
                    {
                        ViewData["UserRole"] = role;
                    }
                    else
                    {
                        ViewData["UserRole"] = "No Role Assigned";
                    }

                    if (await _userManager.IsInRoleAsync(user, "Artist"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Buyer"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}

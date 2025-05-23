﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Models;
using SmartClinic.ViewModels;

namespace SmartClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _hosting;

        public AccountController(IWebHostEnvironment hosting, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _hosting = hosting;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(registerVM userModel)
        {
<<<<<<< HEAD

           if(ModelState.IsValid) {

                AppUser user;
                if (userModel.Role == "Patient")
                {
                user = new Patient();

                }
                else if(userModel.Role=="Doctor")
                {

                    user = new Doctor();

                }
                else
                {
                    user = new AppUser();
                }
                   

                    user.FullName = userModel.Name;
                    user.Email = userModel.Email;
                    user.PhoneNumber = userModel.PhoneNumber;
                   

                    user.DateOfBirth = userModel.DateOfBirth;
                    user.Address = userModel.Address;
                    user.UserName = userModel.userName;
                user.Role = userModel.Role;
                    string file;
                    if (userModel.imageFile != null)
                    {
                        file = Path.Combine(Hosting.WebRootPath, "Images");

                        string FullPath = Path.Combine(file, userModel.imageFile.Name);
                        user.ImagePath = userModel.imageFile.Name;
                        userModel.imageFile.CopyTo(new FileStream(FullPath, FileMode.Create));



                    }


                    IdentityResult res = await userManager.CreateAsync(user,userModel.PassWord);
                    if (res.Succeeded)
                    {

                        await userManager.AddToRoleAsync(user, userModel.Role);
                    }
                  

               


                   


                

             

             






            }

 if(User.IsInRole("Admin"))
            {

                return RedirectToAction("showDoctors", "Admin");
            }
            else
            {
                return View();
            }

        }



        [HttpGet]
        public IActionResult logIn()
        {



            return View();

        }


        [HttpPost]
        public async Task<IActionResult> logIn(LoginVM userModel)
        {

=======
>>>>>>> ae84cc30f4b0777f705b7e74ccee26b8864416da
            if (ModelState.IsValid)
            {
                AppUser user;

                if (userModel.Role == "Doctor")
                {
<<<<<<< HEAD

                    if (await userManager.CheckPasswordAsync(user,userModel.password))
                    {
                     await   Sign.SignInAsync(user, userModel.rememberMe);

                    }
                 



=======
                    user = new Doctor
                    {
                        FullName = userModel.Name,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        DateOfBirth = userModel.DateOfBirth,
                        Address = userModel.Address,
                        UserName = userModel.UserName,
                        Specialization = userModel.Specialization,
                        ExceptionDates = userModel.ExceptionDates,
                        DefaultDate = userModel.DefaultDate,
                        IsAvailable = true,
                        IsDeleted = false
                    };
                }
                else if (userModel.Role == "Receptionist")
                {
                    user = new Receptionist
                    {
                        FullName = userModel.Name,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        DateOfBirth = userModel.DateOfBirth,
                        Address = userModel.Address,
                        UserName = userModel.UserName,
                        IsDeleted = false,
                        Salary = userModel.Salary
                    };
                }
                else // Default to Patient if no role is specified
                {
                    user = new Patient
                    {
                        FullName = userModel.Name,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        DateOfBirth = userModel.DateOfBirth,
                        Address = userModel.Address,
                        UserName = userModel.UserName,
                        IsDeleted = false
                    };
>>>>>>> ae84cc30f4b0777f705b7e74ccee26b8864416da
                }

                if (userModel.imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_hosting.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(userModel.imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userModel.imageFile.CopyToAsync(stream);
                    }

                    user.ImagePath = uniqueFileName;
                }

                var result = await _userManager.CreateAsync(user, userModel.PassWord);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userModel.Role);
                    TempData["Success"] = "User registered successfully.";
                    return RedirectToAction("LogIn");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(userModel);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

<<<<<<< HEAD
        public async Task< IActionResult> delUserAccount(string userId)
        {
            AppUser userModel = await userManager.FindByIdAsync(userId);

            if (userModel != null)
            {

               await userManager.DeleteAsync(userModel);

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("showDoctors", "Admin");
                }
                return Content("the account is deleted");
            }

            return Content("user not found");
        }
       
=======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginVM userModel)
        {
            if (!ModelState.IsValid)
                return View(userModel);

            var user = await _userManager.FindByEmailAsync(userModel.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    userModel.Password,
                    userModel.RememberMe,
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Patient"))
                        return RedirectToAction("Index", "PatientDashboard");

                    if (await _userManager.IsInRoleAsync(user, "Doctor"))
                        return RedirectToAction("Index", "DoctorDashboard");

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn");
        }
>>>>>>> ae84cc30f4b0777f705b7e74ccee26b8864416da
    }
}

﻿using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using CompanyG02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp() 
        {
            return View();
        }

        // P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null) 
                {
                    user = await _userManager.FindByEmailAsync(model.Email);

                    if (user is null) 
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,

                        };
                       
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);

                        }

                    }

                }

                ModelState.AddModelError("", "Invalid SignUp !!");
            }

            return View(model);
        }


        #endregion


        #region SignIn
        [HttpGet]
        public IActionResult SignIn() 
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {

            if (ModelState.IsValid) 
            {
               var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null) 
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag) 
                    {
                       var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe , false);
                        if (result.Succeeded) 
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }
                   
                }

                ModelState.AddModelError("","Invalid Login !");
           
            }


            return View(model);
        }


        #endregion


        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut() 
        {
           await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SignIn));
        }

        #endregion


        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model) 
        {

            if (ModelState.IsValid) 
            {
               var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null) 
                {
                   var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                   var url = Url.Action("ResetPassword","Account",new { email = model.Email , token },Request.Scheme);
                   

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url

                    };


                  var flag =  EmailSettings.SendEmail(email);
                    if (flag) 
                    {

                        return RedirectToAction("CheckYourIndex");
                    }
                }

            }

            ModelState.AddModelError("", "Invalid Reset Password !!");
            return View("ForgetPassword",model);
        }


        [HttpGet]
        public IActionResult CheckYourIndex() 
        {
            return View();
        }


        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email,string token) 
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Operation !");
                var user = await _userManager.FindByEmailAsync(email);

                if (user is not null) 
                {
                   var result=await _userManager.ResetPasswordAsync(user,token,model.NewPassword);

                    if (result.Succeeded) 
                    {
                        return RedirectToAction("SignIn");
                    }
                }

                ModelState.AddModelError("", "Invalid Reset Password Operation");


            }
            return View();
        }


        #endregion



    }
}

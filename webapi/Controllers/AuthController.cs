using CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using webapi.Jwt;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserModel userModel;

        private ModelFactory modelFactory;
        public LoginController(ILogger<LoginController> logger, SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager,IJwtGenerator jwtGenerator,UserModel _usermodel)
        {
            _logger = logger;
            _signinManager = signinManager;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            modelFactory = new ModelFactory();
            userModel = _usermodel;

            if (userModel.GetUsers().Count() < _userManager.Users.Count())
            {
                userModel.migrate(ApplicationUser.getusernames(_userManager.Users));

            }

        }


        [Route("/register")]
        [HttpPost]
        public async Task<string> register(RegisterModel model)
        {

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Nickname = model.Nickname,

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                modelFactory.UserModel.AddUser(model.UserName);
                var SignIn = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);

                if (SignIn.Succeeded) return "succsess";
                else return "SignIn Error";
            }

            return "HELLPLPL";

        }


        [Route("/signin")]
        [HttpPost]
        public async Task<IActionResult> signin([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var SignIn = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);
            if (SignIn.Succeeded) 
            {


                var response = new
                {
                    access_token = _jwtGenerator.CreateToken(user),
                    username = user.UserName,
                };

                return Json(response);



            }
            else return Json("SignIn Error");

        }

        [Authorize]
        [Route("/signout")]
        [HttpGet]
        public async Task<IActionResult> signout()
        {
            await _signinManager.SignOutAsync();
            return Json("succsesfly signed out");
        }

        [Route("/about")]
        [HttpGet]
        public IActionResult about()
        {
            return Json(_userManager.Users.ToList());
        }





    }

}


  

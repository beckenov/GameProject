using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GnsGameProject.Common;
using GnsGameProject.Service.Common.UserService;
using GnsGameProject.Service.Common.UserService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GnsGameProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            var user = userService.GetUsers() ?? new List<UserModel>();
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public IActionResult Create()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Details(Guid userId)
        {
            var userStatistic = await userService.Details(userId);
            return View(userStatistic);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Create(UserModel model)
        {
            var result =  await userService.Create(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                ViewBag.Result = result.Message;
                return this.View();
            }
           
        }
        
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Remove(Guid id)
        {
            await userService.Remove(id);
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userModel = await userService.PrepeareUserForEditView(id);
            return this.View(userModel);
            
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Edit(UserModel model)
        {
            await userService.PrepeareUserForEditAsync(model);
            return RedirectToAction("Index", "User");
        }

    }
}

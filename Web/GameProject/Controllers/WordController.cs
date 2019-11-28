using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GnsGameProject.Common;
using GnsGameProject.Service.Common.UserService.Models;
using GnsGameProject.Service.Common.WordService;
using GnsGameProject.Service.Common.WordService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GnsGameProject.Controllers
{
    public class WordController : Controller
    {
        private readonly IWordService wordService;

        public WordController(IWordService wordService)
        {
            this.wordService = wordService;
        }

        public IActionResult Index()
        {
            var words = wordService.GetWords() ?? new List<WordModel>();
            return View(words);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Create(WordModel model)
        {

                await wordService.Create(model);
           
           
            return RedirectToAction("Index", "Word");
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await wordService.PrepeareWordForEditView(id);
            return this.View(model);

        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Edit(WordModel model)
        {
            await wordService.PrepeareWordForEditAsync(model);
            return RedirectToAction("Index", "Word");
        }


        [Authorize(Roles = GlobalConstants.Roles.Administrator)]
        public async Task<IActionResult> Remove(Guid id)
        {
            await wordService.RemoveAsync(id);
            return RedirectToAction("Index", "Word");
        }
    }
}

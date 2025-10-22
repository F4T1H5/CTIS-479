using APP.Models;
using APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Obsolete("Use GroupsController class instead!")]
    public class GroupsObsoleteController : Controller
    {
        private readonly GroupObsoleteService _groupService;

        public GroupsObsoleteController(GroupObsoleteService groupService)
        {
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            var list = _groupService.Query().ToList();

            ViewBag.Count = list.Count == 0 ? "No groups found!" : list.Count == 1 ? "1 group found." : $"{list.Count} groups found.";

            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = _groupService.Query().SingleOrDefault(groupResponse => groupResponse.Id == id);

            if (item is null)
                ViewBag.Message = "Group not found!";

            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _groupService.Create(request);

                if (response.IsSuccessful)
                {
                    TempData["Message"] = response.Message;

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _groupService.Edit(id);

            if (request is null)
                ViewBag.Message = "Group not found!";

            return View(request);
        }

        [HttpPost]
        public IActionResult Edit(GroupRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _groupService.Update(request);

                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                ViewBag.Message = response.Message;
            }

            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var response = _groupService.Delete(id);

            TempData["Message"] = response.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
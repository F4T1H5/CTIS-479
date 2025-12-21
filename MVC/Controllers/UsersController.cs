#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        private readonly IService<RoleRequest, RoleResponse> _RoleService;

        public UsersController(
            IService<UserRequest, UserResponse> userService
            , IService<GroupRequest, GroupResponse> groupService
            , IService<RoleRequest, RoleResponse> RoleService
        )
        {
            _userService = userService;
            _groupService = groupService;
            _RoleService = RoleService;
        }

        private void SetViewData()
        {
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title");

            ViewBag.RoleIds = new MultiSelectList(_RoleService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        [Authorize]
        public IActionResult Index()
        {
            var list = _userService.List();
            return View(list);
        }

        bool IsOwnAccount(int id)
        {
            return id.ToString() == (User.Claims.SingleOrDefault(claim => claim.Type == "Id")?.Value ?? string.Empty);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            return View(item);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            SetViewData();

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(UserRequest user)
        {
            if (!User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _userService.Create(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();

            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Edit(id);
            SetViewData();

            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(UserRequest user)
        {
            if (!IsOwnAccount(user.Id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.Remove(nameof(UserRequest.Password));
            }

            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                var currentUserId = int.Parse(User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value ?? "0");
                
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    var existingUser = userService.Edit(user.Id);
                    user.Password = existingUser.Password;
                }
                
                var response = await userService.UpdateWithRefresh(user, currentUserId);
                
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();

            return View(user);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var item = _userService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsOwnAccount(id) && !User.IsInRole("Admin"))
            {
                SetTempData("You are not authorized for this operation!");
                return RedirectToAction(nameof(Index));
            }

            var response = _userService.Delete(id);
            SetTempData(response.Message);

            if (IsOwnAccount(id))
                return RedirectToAction(nameof(Logout));

            return RedirectToAction(nameof(Index));
        }



        [Route("~/[action]")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("~/[action]")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;

                var response = await userService.Login(request);
                if (response.IsSuccessful)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", response.Message);
            }
            return View();
        }

        [Route("~/[action]")]
        public async Task<IActionResult> Logout()
        {
            var userService = _userService as UserService;

            await userService.Logout();
            return RedirectToAction(nameof(Login));
        }

        [Route("~/[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("~/[action]")]
        public IActionResult Register(UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                                                              
                var response = userService.Register(request);
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Login));
                ModelState.AddModelError("", response.Message);
            }
            return View(request);
        }

        [Route("~/[action]")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
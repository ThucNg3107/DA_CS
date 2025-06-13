using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Models;
using System.Linq;
using System.Threading.Tasks;
using QuanLyThuVien.Areas.Admin.Models;

namespace QuanLyThuVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<HuyUser> _userManager;

        public UserController(UserManager<HuyUser> userManager)
        {
            _userManager = userManager;
        }

        // Hiển thị danh sách người dùng, có tìm kiếm theo tên hoặc email
        public async Task<IActionResult> Index(string searchString = "")
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.FullName.Contains(searchString) || u.Email.Contains(searchString));
            }

            var userList = users.ToList();

            return View(userList);
        }


        // Xóa người dùng
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            // Tìm người dùng cần xóa
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Xóa người dùng
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Người dùng đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
            }

            return RedirectToAction("Index");
        }


        // Các action Thêm, Sửa, Xóa, Đổi vai trò sẽ bổ sung sau
    }
} 
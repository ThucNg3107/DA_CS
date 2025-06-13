using Microsoft.AspNetCore.Authorization; // Dùng để xác thực và phân quyền
using Microsoft.AspNetCore.Mvc; // Dùng cho các Action và Controller trong ASP.NET Core MVC
using QuanLyThuVien.Areas.Admin.Models; // Đưa các model từ khu vực Admin vào controller

namespace QuanLyThuVien.Controllers // Namespace chứa controller
{
    public class AdminController : Controller // Controller cho khu vực Admin
    {
        // Đảm bảo chỉ người có vai trò Admin mới có thể truy cập trang này
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            return View(); // Trả về view chính cho Admin
        }
    }
}

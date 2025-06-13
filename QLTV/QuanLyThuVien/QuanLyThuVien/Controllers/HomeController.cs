using Microsoft.AspNetCore.Mvc; // Cung cấp các lớp cần thiết cho ASP.NET Core MVC
using QuanLyThuVien.Models; // Đưa các model từ dự án vào controller
using System.Diagnostics; // Dùng để theo dõi hoạt động của ứng dụng, như lỗi và các sự kiện

namespace QuanLyThuVien.Controllers // Namespace chứa controller
{
    public class HomeController : Controller // Controller chính của trang Home
    {
        private readonly ILogger<HomeController> _logger; // Đối tượng logger để ghi lại các thông tin lỗi hoặc sự kiện

        // Constructor của HomeController, nhận logger để theo dõi lỗi và sự kiện
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; // Khởi tạo logger từ dependency injection
        }

        // Hành động này trả về trang chủ (View) khi người dùng truy cập vào URL gốc
        public IActionResult Index()
        {
            return View(); // Trả về view cho trang chủ (Index)
        }

        // Hành động này trả về trang Privacy khi người dùng truy cập vào URL /Privacy
        public IActionResult Privacy()
        {
            return View(); // Trả về view cho trang Privacy
        }

        // Hành động này xử lý lỗi và trả về trang lỗi nếu có sự cố trong ứng dụng
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tắt cache cho trang lỗi
        public IActionResult Error()
        {
            // Trả về view lỗi, với thông tin ID yêu cầu hoặc mã theo dõi lỗi
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

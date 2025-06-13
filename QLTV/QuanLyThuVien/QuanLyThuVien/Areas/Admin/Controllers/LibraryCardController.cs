using Microsoft.AspNetCore.Authorization; // Thư viện xử lý quyền truy cập
using Microsoft.AspNetCore.Mvc; // Thư viện cho Controller và Action
using QuanLyThuVien.Areas.Admin.Models; // Các model liên quan đến Admin
using QuanLyThuVien.Models; // Các model chung
using QuanLyThuVien.Repositories; // Các interface repository
using System.Linq; // Dùng để truy vấn dữ liệu
using System.Threading.Tasks; // Dùng để xử lý các tác vụ bất đồng bộ

namespace QuanLyThuVien.Areas.Admin.Controllers // Định danh namespace của Controller
{
    [Area("Admin")] // Đánh dấu đây là controller trong khu vực Admin
    [Authorize(Roles = SD.Role_Admin)] // Chỉ cho phép người dùng có vai trò Admin truy cập controller này
    public class LibraryCardController : Controller // Controller quản lý thẻ thư viện
    {
        private readonly ILibraryCardRepository _repository; // Interface quản lý thẻ thư viện

        // Constructor: Inject repository vào controller để sử dụng
        public LibraryCardController(ILibraryCardRepository repository)
        {
            _repository = repository; // Gán repository vào biến
        }

        // Hiển thị danh sách tất cả thẻ thư viện
        public async Task<IActionResult> Index()
        {
            var cards = await _repository.GetAllAsync(); // Lấy tất cả thẻ thư viện từ DB
            return View(cards); // Trả về view và truyền danh sách thẻ thư viện
        }

        // Hiển thị form đăng ký thẻ thư viện
        public IActionResult Register() => View(); // Trả về form đăng ký

        // Xử lý khi người dùng gửi form đăng ký thẻ thư viện
        [HttpPost] // Phương thức POST
        [ValidateAntiForgeryToken] // Kiểm tra tính hợp lệ của form
        public async Task<IActionResult> Register(LibraryCard card)
        {
            if (card.ExpiryDate <= card.IssuedDate)
            {
                ModelState.AddModelError("ExpiryDate", "Ngày hết hạn phải lớn hơn ngày cấp thẻ");
            }

            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu form
            {
                await _repository.AddAsync(card); // Thêm thẻ thư viện vào DB
                TempData["Success"] = "Đăng ký thẻ thư viện thành công!"; // Lưu thông báo thành công
                return RedirectToAction(nameof(Index)); // Quay lại trang danh sách thẻ
            }
            return View(card); // Nếu dữ liệu không hợp lệ, trả về lại form đăng ký
        }

        // Hiển thị form cập nhật thẻ thư viện
        public async Task<IActionResult> Update(int id)
        {
            var card = await _repository.GetByIdAsync(id); // Lấy thẻ thư viện theo ID
            if (card == null)
            {
                return NotFound(); // Nếu không tìm thấy thẻ, trả về lỗi 404
            }
            return View(card); // Trả về form cập nhật với dữ liệu của thẻ
        }

        // Xử lý khi người dùng gửi form cập nhật thẻ thư viện
        [HttpPost] // Phương thức POST
        [ValidateAntiForgeryToken] // Kiểm tra tính hợp lệ của form
        public async Task<IActionResult> Update(int id, LibraryCard card)
        {
            if (id != card.CardId) // Kiểm tra ID trong URL có khớp với ID trong form không
            {
                return BadRequest(); // Nếu không khớp, trả về lỗi 400 (Bad Request)
            }

            if (card.ExpiryDate <= card.IssuedDate)
            {
                ModelState.AddModelError("ExpiryDate", "Ngày hết hạn phải lớn hơn ngày cấp thẻ");
            }

            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu form
            {
                await _repository.UpdateAsync(card); // Cập nhật thẻ thư viện trong DB
                TempData["Success"] = "Cập nhật thẻ thư viện thành công!"; // Thông báo thành công
                return RedirectToAction(nameof(Index)); // Quay lại trang danh sách
            }
            return View(card); // Nếu dữ liệu không hợp lệ, trả về lại form cập nhật
        }

        // Hiển thị form xác nhận xóa thẻ thư viện
        public async Task<IActionResult> Delete(int id)
        {
            var card = await _repository.GetByIdAsync(id); // Lấy thẻ thư viện theo ID
            if (card == null)
            {
                return NotFound(); // Nếu không tìm thấy thẻ, trả về lỗi 404
            }
            return View(card); // Trả về form xác nhận xóa
        }

        // Xử lý khi xác nhận xóa thẻ thư viện
        [HttpPost, ActionName("Delete")] // Phương thức POST và yêu cầu người dùng xác nhận xóa
        [ValidateAntiForgeryToken] // Kiểm tra tính hợp lệ của form
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id); // Xóa thẻ thư viện khỏi DB
            TempData["Success"] = "Xóa thẻ thư viện thành công!"; // Thông báo thành công
            return RedirectToAction(nameof(Index)); // Quay lại trang danh sách
        }
    }
}

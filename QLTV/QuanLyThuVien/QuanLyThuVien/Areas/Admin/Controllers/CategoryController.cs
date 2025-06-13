using Microsoft.AspNetCore.Authorization; // Thư viện để xử lý quyền truy cập
using Microsoft.AspNetCore.Mvc; // Thư viện cho Controller và Action
using QuanLyThuVien.Areas.Admin.Models; // Model cho khu vực Admin
using QuanLyThuVien.Models; // Các model chung
using QuanLyThuVien.Repositories; // Các interface repository
using System.Threading.Tasks; // Dùng để xử lý các tác vụ bất đồng bộ

namespace QuanLyThuVien.Areas.Admin.Controllers // Định danh namespace của Controller
{
    [Area("Admin")] // Đánh dấu đây là controller trong khu vực Admin
    [Authorize(Roles = SD.Role_Admin)] // Chỉ cho phép người dùng có vai trò Admin truy cập
    public class CategoryController : Controller // Controller quản lý danh mục
    {
        private readonly ICategoryRepository _categoryRepository; // Interface quản lý danh mục

        // Constructor: Inject repository vào controller để sử dụng trong các action
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository; // Gán repository vào biến
        }

        // Hiển thị danh sách tất cả danh mục
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync(); // Lấy tất cả danh mục từ DB
            return View(categories); // Truyền danh sách vào view để hiển thị
        }

        // Hiển thị form thêm danh mục
        public IActionResult Add() => View(); // Trả về form thêm danh mục

        // Xử lý khi người dùng gửi form thêm danh mục
        [HttpPost] // Phương thức POST
        public async Task<IActionResult> Add(Category category)
        {
            if (!ModelState.IsValid) return View(category); // Kiểm tra nếu dữ liệu không hợp lệ thì hiển thị lại form

            await _categoryRepository.AddAsync(category); // Thêm danh mục mới vào DB thông qua repository
            return RedirectToAction(nameof(Index)); // Quay lại trang danh sách sau khi thêm thành công
        }

        // Hiển thị chi tiết danh mục (chỉ xem, không chỉnh sửa)
        public async Task<IActionResult> Display(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); // Lấy danh mục theo ID
            return category == null ? NotFound() : View(category); // Trả về view với chi tiết hoặc trả lỗi 404 nếu không tồn tại
        }

        // Hiển thị form cập nhật danh mục
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); // Lấy danh mục cần cập nhật
            return category == null ? NotFound() : View(category); // Nếu không tìm thấy thì trả lỗi 404
        }

        // Xử lý khi người dùng gửi form cập nhật danh mục
        [HttpPost] // Phương thức POST
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.CategoryId) return NotFound(); // Kiểm tra nếu ID từ form không khớp với ID trong URL thì báo lỗi
            if (!ModelState.IsValid) return View(category); // Kiểm tra nếu dữ liệu không hợp lệ thì hiển thị lại form

            var existingCategory = await _categoryRepository.GetByIdAsync(id); // Lấy danh mục hiện tại từ DB
            if (existingCategory == null) return NotFound(); // Nếu không tồn tại trong DB thì trả lỗi 404

            // Cập nhật thông tin danh mục
            existingCategory.Name = category.Name;
            await _categoryRepository.UpdateAsync(existingCategory); // Cập nhật thông tin vào DB qua repository

            return RedirectToAction(nameof(Index)); // Quay lại danh sách sau khi cập nhật thành công
        }

        // Hiển thị form xác nhận xóa danh mục
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); // Lấy danh mục cần xóa
            return category == null ? NotFound() : View(category); // Nếu không tồn tại thì trả lỗi 404
        }

        // Xử lý khi xác nhận xóa danh mục
        [HttpPost, ActionName("Delete")] // Phương thức POST và yêu cầu người dùng xác nhận
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepository.DeleteAsync(id); // Gọi repository để xóa danh mục khỏi DB
            return RedirectToAction(nameof(Index)); // Quay lại trang danh sách sau khi xóa
        }
    }
}

using Microsoft.AspNetCore.Mvc; // Thư viện cho Controller và các Action
using QuanLyThuVien.Repositories; // Gọi đến các interface repository
using System.Threading.Tasks; // Cho phép sử dụng async/await
using Microsoft.AspNetCore.Mvc.Rendering; // Dùng để tạo SelectListItem (dropdown)
using Microsoft.AspNetCore.Hosting; // Dùng để truy xuất thông tin môi trường hosting
using System.IO; // Dùng để thao tác với file (đường dẫn, ghi file, xóa file,...)
using Microsoft.AspNetCore.Http; // Để xử lý file upload (IFormFile)
using Microsoft.AspNetCore.Authorization; // Quản lý quyền truy cập
using QuanLyThuVien.Areas.Admin.Models; // Model thuộc khu vực Admin
using QuanLyThuVien.Models; // Gọi các model chung
using System.Linq;

namespace QuanLyThuVien.Areas.Admin.Controllers // Định danh namespace của Controller
{
    [Area("Admin")] // Controller thuộc khu vực Admin
    [Authorize(Roles = SD.Role_Admin)] // Chỉ Admin mới được phép truy cập
    public class BookController : Controller // Controller quản lý chức năng sách
    {
        // Khai báo các biến dùng để truy cập database hoặc môi trường
        private readonly IBookRepository _bookRepository; // Repository quản lý sách
        private readonly ICategoryRepository _categoryRepository; // Repository danh mục
        private readonly IWebHostEnvironment _webHostEnvironment; // Lấy đường dẫn wwwroot

        // Constructor để inject các repository và môi trường
        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository; // Gán repository sách
            _categoryRepository = categoryRepository; // Gán repository danh mục
            _webHostEnvironment = webHostEnvironment; // Gán môi trường web
        }

        // GET: Hiển thị danh sách tất cả sách
        public async Task<IActionResult> Index(string searchString)
        {
            var books = await _bookRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = books.Where(b =>
                    (!string.IsNullOrEmpty(b.Name) && b.Name.ToLower().Contains(searchString)) ||
                    (b.Category != null && !string.IsNullOrEmpty(b.Category.Name) && b.Category.Name.ToLower().Contains(searchString)) ||
                    ($"Dãy {b.ShelfRow} - Kệ {b.ShelfNumber}".ToLower().Contains(searchString))
                ).ToList();
            }
            return View(books);
        }

        // GET: Hiển thị chi tiết 1 quyển sách
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id); // Lấy sách theo ID
            if (book == null) return NotFound(); // Nếu không tìm thấy sách thì trả lỗi 404
            return View(book); // Trả về view chi tiết sách
        }

        // GET: Hiển thị form thêm sách
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync(); // Lấy danh sách danh mục
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(), // Giá trị dropdown là CategoryId
                Text = c.Name // Tên hiển thị
            }).ToList();
            return View(); // Trả về form thêm sách
        }

        // POST: Xử lý khi người dùng thêm sách mới
        [HttpPost]
        [ValidateAntiForgeryToken] // Ngăn chặn CSRF
        public async Task<IActionResult> Add(Book book, IFormFile ImageFile)
        {
            if (!ModelState.IsValid) // Kiểm tra model hợp lệ
            {
                var categories = await _categoryRepository.GetAllAsync(); // Load lại danh mục nếu model sai
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
                return View(book); // Trả về form với lỗi
            }

            // Nếu có upload ảnh bìa sách
            if (ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Lấy đường dẫn wwwroot/images
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName; // Tạo tên file duy nhất
                string filePath = Path.Combine(uploadsFolder, uniqueFileName); // Đường dẫn đầy đủ lưu ảnh

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream); // Ghi file ảnh vào server
                }

                book.ImageUrl = "/images/" + uniqueFileName; // Lưu đường dẫn ảnh vào DB
            }

            await _bookRepository.AddAsync(book); // Gọi hàm thêm sách
            return RedirectToAction(nameof(Index)); // Quay về trang danh sách
        }

        // GET: Hiển thị form chỉnh sửa sách
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id); // Lấy sách theo ID
            if (book == null) return NotFound(); // Nếu không có sách thì báo lỗi

            var categories = await _categoryRepository.GetAllAsync(); // Lấy danh mục để chọn lại
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            return View(book); // Trả view chỉnh sửa
        }

        // POST: Xử lý khi lưu chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile ImageFile = null)
        {
            if (id != book.BookId) return BadRequest(); // Kiểm tra ID đúng không

            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllAsync(); // Nếu lỗi thì load lại danh mục
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
                return View(book); // Trả về view với lỗi
            }

            var existingBook = await _bookRepository.GetByIdAsync(id); // Lấy sách cần chỉnh sửa
            if (existingBook == null) return NotFound(); // Không tìm thấy thì lỗi

            // Gán lại thông tin sách
            existingBook.Name = book.Name;
            existingBook.Author = book.Author;
            existingBook.Publisher = book.Publisher;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.CategoryId = book.CategoryId;
            existingBook.BookStatus = book.BookStatus;
            // Thêm cập nhật vị trí và số lượng
            existingBook.ShelfRow = book.ShelfRow;
            existingBook.ShelfNumber = book.ShelfNumber;
            existingBook.Quantity = book.Quantity;

            // Chỉ cập nhật ảnh nếu có ảnh mới được upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingBook.ImageUrl))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingBook.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingBook.ImageUrl = "/images/" + uniqueFileName;
            }
            else
            {
                // Giữ nguyên ảnh cũ nếu không có ảnh mới
                existingBook.ImageUrl = book.ImageUrl;
            }

            await _bookRepository.UpdateAsync(existingBook); // Lưu vào DB
            return RedirectToAction(nameof(Index)); // Trả về danh sách
        }

        // GET: Hiển thị xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id); // Lấy sách cần xóa
            if (book == null) return NotFound(); // Không tồn tại thì báo lỗi
            return View(book); // Trả về view xác nhận
        }

        // POST: Xử lý xóa sách
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id); // Lấy sách

            if (book != null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath); // Xóa ảnh trên máy chủ
                        }
                        catch
                        {
                            // Ghi log nếu xóa thất bại (tùy chọn)
                        }
                    }
                }

                await _bookRepository.DeleteAsync(id); // Xóa sách khỏi DB
            }

            return RedirectToAction(nameof(Index)); // Trở về danh sách
        }
    }
}

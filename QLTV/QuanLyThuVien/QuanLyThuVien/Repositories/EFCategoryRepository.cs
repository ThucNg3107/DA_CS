using Microsoft.EntityFrameworkCore;  // Cung cấp các lớp và phương thức hỗ trợ làm việc với Entity Framework (EF) Core.
using System.Data.SqlTypes;  // Được sử dụng để làm việc với các kiểu dữ liệu SQL (không có vẻ cần thiết trong đoạn mã này).
using QuanLyThuVien.Models;  // Chứa các mô hình dữ liệu trong ứng dụng, bao gồm các lớp như Category, Book, Loan, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Lớp EFCategoryRepository triển khai ICategoryRepository để thực hiện các thao tác với bảng Category trong cơ sở dữ liệu.
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly HuyDBContext _context;  // Khai báo một biến để lưu trữ ngữ cảnh của cơ sở dữ liệu (HuyDBContext).

        // Constructor nhận đối tượng HuyDBContext để kết nối với cơ sở dữ liệu.
        public EFCategoryRepository(HuyDBContext context)
        {
            _context = context;  // Khởi tạo _context bằng đối tượng context được truyền vào từ bên ngoài.
        }

        // Phương thức lấy tất cả các danh mục từ cơ sở dữ liệu.
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Sử dụng EF Core để lấy tất cả danh mục từ bảng Categories.
            return await _context.Categories.ToListAsync();  // Chuyển kết quả thành danh sách và trả về.
        }

        // Phương thức lấy danh mục theo ID từ cơ sở dữ liệu.
        public async Task<Category?> GetByIdAsync(int id)
        {
            // Tìm danh mục với CategoryId = id.
            return await _context.Categories.FindAsync(id);  // Trả về danh mục tìm thấy hoặc null nếu không tìm thấy.
        }

        // Phương thức thêm một danh mục mới vào cơ sở dữ liệu.
        public async Task AddAsync(Category category)
        {
            // Thêm danh mục vào bảng Categories.
            await _context.Categories.AddAsync(category);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }

        // Phương thức cập nhật thông tin danh mục trong cơ sở dữ liệu.
        public async Task UpdateAsync(Category category)
        {
            // Cập nhật thông tin danh mục trong bảng Categories.
            _context.Categories.Update(category);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }

        // Phương thức xóa danh mục theo ID từ cơ sở dữ liệu.
        public async Task DeleteAsync(int id)
        {
            // Tìm danh mục theo ID.
            var category = await _context.Categories.FindAsync(id);
            if (category != null)  // Nếu tìm thấy danh mục.
            {
                // Xóa danh mục khỏi bảng Categories.
                _context.Categories.Remove(category);
                // Lưu thay đổi vào cơ sở dữ liệu.
                await _context.SaveChangesAsync();
            }
        }
    }
}

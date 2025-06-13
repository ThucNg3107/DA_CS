using Microsoft.EntityFrameworkCore;  // Cung cấp các lớp và phương thức hỗ trợ làm việc với Entity Framework (EF) Core.
using QuanLyThuVien.Models;  // Thư viện mô hình dữ liệu trong ứng dụng quản lý thư viện.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Lớp EFBookRepository triển khai IBookRepository để thực hiện các thao tác với bảng Book trong cơ sở dữ liệu
    public class EFBookRepository : IBookRepository
    {
        private readonly HuyDBContext _context;  // Khai báo một biến để lưu trữ ngữ cảnh của cơ sở dữ liệu (HuyDBContext).

        // Constructor nhận đối tượng HuyDBContext để kết nối với cơ sở dữ liệu.
        public EFBookRepository(HuyDBContext context)
        {
            _context = context;  // Khởi tạo _context bằng đối tượng context được truyền vào từ bên ngoài.
        }

        // Phương thức lấy tất cả sách từ cơ sở dữ liệu.
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            // Sử dụng EF Core để lấy tất cả sách từ bảng Books, bao gồm Category của mỗi sách.
            return await _context.Books
                .Include(b => b.Category)  // Load Category liên quan để tránh null.
                .ToListAsync();  // Chuyển kết quả thành danh sách và trả về.
        }

        // Phương thức lấy sách theo ID từ cơ sở dữ liệu.
        public async Task<Book?> GetByIdAsync(int id)
        {
            // Tìm sách với BookId = id, bao gồm Category liên quan.
            return await _context.Books
                .Include(b => b.Category)  // Load Category liên quan để tránh null.
                .FirstOrDefaultAsync(b => b.BookId == id);  // Trả về sách đầu tiên hoặc null nếu không tìm thấy.
        }

        // Phương thức thêm một sách mới vào cơ sở dữ liệu.
        public async Task AddAsync(Book book)
        {
            // Thêm sách vào bảng Books.
            await _context.Books.AddAsync(book);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }

        // Phương thức cập nhật thông tin sách trong cơ sở dữ liệu.
        public async Task UpdateAsync(Book book)
        {
            // Cập nhật thông tin sách trong bảng Books.
            _context.Books.Update(book);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }

        // Phương thức xóa sách theo ID từ cơ sở dữ liệu.
        public async Task DeleteAsync(int id)
        {
            // Tìm sách theo ID.
            var book = await _context.Books.FindAsync(id);
            if (book != null)  // Nếu tìm thấy sách.
            {
                // Xóa sách khỏi bảng Books.
                _context.Books.Remove(book);
                // Lưu thay đổi vào cơ sở dữ liệu.
                await _context.SaveChangesAsync();
            }
        }
    }
}

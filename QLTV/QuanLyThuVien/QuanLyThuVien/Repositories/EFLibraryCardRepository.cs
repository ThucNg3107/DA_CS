using Microsoft.EntityFrameworkCore;  // Cung cấp các lớp và phương thức hỗ trợ làm việc với Entity Framework (EF) Core.
using System.Collections.Generic;  // Cung cấp các kiểu dữ liệu Collection như List, IEnumerable, v.v.
using System.Linq;  // Cung cấp các phương thức LINQ để truy vấn dữ liệu.
using System.Threading.Tasks;  // Hỗ trợ các tác vụ bất đồng bộ (asynchronous).
using QuanLyThuVien.Models;  // Chứa các mô hình dữ liệu trong ứng dụng, bao gồm các lớp như LibraryCard, Book, Fine, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Lớp EFLibraryCardRepository triển khai ILibraryCardRepository để thực hiện các thao tác với bảng LibraryCards trong cơ sở dữ liệu.
    public class EFLibraryCardRepository : ILibraryCardRepository
    {
        private readonly HuyDBContext _context;  // Khai báo một biến để lưu trữ ngữ cảnh của cơ sở dữ liệu (HuyDBContext).

        // Constructor nhận đối tượng HuyDBContext để kết nối với cơ sở dữ liệu.
        public EFLibraryCardRepository(HuyDBContext context)
        {
            _context = context;  // Khởi tạo _context bằng đối tượng context được truyền vào từ bên ngoài.
        }

        // Phương thức lấy tất cả các thẻ thư viện từ cơ sở dữ liệu.
        public async Task<IEnumerable<LibraryCard>> GetAllAsync()
        {
            // Lấy tất cả thẻ thư viện từ bảng LibraryCards và bao gồm danh sách Loans
            return await _context.LibraryCards
                .Include(c => c.Loans)
                .ToListAsync();
        }

        // Phương thức lấy thẻ thư viện theo ID từ cơ sở dữ liệu.
        public async Task<LibraryCard?> GetByIdAsync(int id)
        {
            // Tìm thẻ thư viện với LibraryCardId = id.
            return await _context.LibraryCards.FindAsync(id);
        }

        // Phương thức thêm một thẻ thư viện mới vào cơ sở dữ liệu.
        public async Task AddAsync(LibraryCard card)
        {
            // Thêm thẻ thư viện vào bảng LibraryCards.
            await _context.LibraryCards.AddAsync(card);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await SaveAsync();
        }

        // Phương thức cập nhật thông tin thẻ thư viện trong cơ sở dữ liệu.
        public async Task UpdateAsync(LibraryCard card)
        {
            // Cập nhật thông tin thẻ thư viện trong bảng LibraryCards.
            _context.LibraryCards.Update(card);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await SaveAsync();
        }

        // Phương thức xóa thẻ thư viện theo ID từ cơ sở dữ liệu.
        public async Task DeleteAsync(int id)
        {
            // Lấy thẻ thư viện theo ID.
            var card = await GetByIdAsync(id);
            if (card != null)  // Nếu tìm thấy thẻ thư viện.
            {
                // Xóa thẻ thư viện khỏi bảng LibraryCards.
                _context.LibraryCards.Remove(card);
                // Lưu thay đổi vào cơ sở dữ liệu.
                await SaveAsync();
            }
        }

        // Phương thức lưu các thay đổi vào cơ sở dữ liệu.
        public async Task SaveAsync()
        {
            // Lưu tất cả thay đổi trong ngữ cảnh _context vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }
    }
}

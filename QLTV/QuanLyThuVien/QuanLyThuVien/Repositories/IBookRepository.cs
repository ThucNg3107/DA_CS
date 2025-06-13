using QuanLyThuVien.Models;  // Cung cấp các mô hình dữ liệu như Book, Category, Fine, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Giao diện IBookRepository định nghĩa các phương thức tương tác với dữ liệu sách (Book).
    public interface IBookRepository
    {
        // Phương thức bất đồng bộ để lấy tất cả các sách từ cơ sở dữ liệu.
        Task<IEnumerable<Book>> GetAllAsync();

        // Phương thức bất đồng bộ để lấy một cuốn sách theo ID từ cơ sở dữ liệu.
        Task<Book> GetByIdAsync(int id);

        // Phương thức bất đồng bộ để thêm một cuốn sách mới vào cơ sở dữ liệu.
        Task AddAsync(Book book);

        // Phương thức bất đồng bộ để cập nhật thông tin một cuốn sách trong cơ sở dữ liệu.
        Task UpdateAsync(Book book);

        // Phương thức bất đồng bộ để xóa một cuốn sách theo ID từ cơ sở dữ liệu.
        Task DeleteAsync(int id);
    }
}

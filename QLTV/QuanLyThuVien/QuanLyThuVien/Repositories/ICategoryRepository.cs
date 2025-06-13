using QuanLyThuVien.Models;  // Cung cấp các mô hình dữ liệu như Category, Book, Fine, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Giao diện ICategoryRepository định nghĩa các phương thức tương tác với dữ liệu thể loại sách (Category).
    public interface ICategoryRepository
    {
        // Phương thức bất đồng bộ để lấy tất cả các thể loại sách từ cơ sở dữ liệu.
        Task<IEnumerable<Category>> GetAllAsync();

        // Phương thức bất đồng bộ để lấy một thể loại sách theo ID từ cơ sở dữ liệu.
        Task<Category> GetByIdAsync(int id);

        // Phương thức bất đồng bộ để thêm một thể loại sách mới vào cơ sở dữ liệu.
        Task AddAsync(Category category);

        // Phương thức bất đồng bộ để cập nhật thông tin một thể loại sách trong cơ sở dữ liệu.
        Task UpdateAsync(Category category);

        // Phương thức bất đồng bộ để xóa một thể loại sách theo ID từ cơ sở dữ liệu.
        Task DeleteAsync(int id);
    }
}

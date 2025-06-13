using QuanLyThuVien.Models;  // Cung cấp các mô hình dữ liệu như LibraryCard, v.v.
using System.Collections.Generic;  // Cung cấp các kiểu dữ liệu danh sách, ví dụ: IEnumerable.
using System.Threading.Tasks;  // Cung cấp các kiểu bất đồng bộ như Task.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Giao diện ILibraryCardRepository định nghĩa các phương thức để tương tác với dữ liệu thẻ thư viện (LibraryCard).
    public interface ILibraryCardRepository
    {
        // Phương thức bất đồng bộ để lấy tất cả các thẻ thư viện từ cơ sở dữ liệu.
        Task<IEnumerable<LibraryCard>> GetAllAsync();

        // Phương thức bất đồng bộ để lấy một thẻ thư viện theo ID từ cơ sở dữ liệu.
        Task<LibraryCard?> GetByIdAsync(int id);

        // Phương thức bất đồng bộ để thêm một thẻ thư viện mới vào cơ sở dữ liệu.
        Task AddAsync(LibraryCard card);

        // Phương thức bất đồng bộ để cập nhật thông tin của một thẻ thư viện trong cơ sở dữ liệu.
        Task UpdateAsync(LibraryCard card);

        // Phương thức bất đồng bộ để xóa một thẻ thư viện khỏi cơ sở dữ liệu theo ID.
        Task DeleteAsync(int id);

        // Phương thức bất đồng bộ để lưu tất cả các thay đổi chưa được ghi vào cơ sở dữ liệu.
        Task SaveAsync();
    }
}

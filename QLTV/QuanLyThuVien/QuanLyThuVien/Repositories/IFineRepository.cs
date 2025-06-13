using QuanLyThuVien.Models;  // Cung cấp các mô hình dữ liệu như Fine, Loan, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Giao diện IFineRepository định nghĩa các phương thức tương tác với dữ liệu tiền phạt (Fine).
    public interface IFineRepository
    {
        // Phương thức bất đồng bộ để lấy tất cả các tiền phạt từ cơ sở dữ liệu.
        Task<IEnumerable<Fine>> GetAllAsync();

        // Phương thức bất đồng bộ để lấy một tiền phạt theo ID từ cơ sở dữ liệu.
        Task<Fine?> GetByIdAsync(int id);

        // Phương thức bất đồng bộ để thêm một tiền phạt mới vào cơ sở dữ liệu.
        Task<Fine> AddAsync(Fine fine);

        // Phương thức bất đồng bộ để cập nhật thông tin một tiền phạt trong cơ sở dữ liệu.
        Task UpdateAsync(Fine fine);

        // Phương thức bất đồng bộ để xóa một tiền phạt khỏi cơ sở dữ liệu.
        Task DeleteAsync(Fine fine);

        // Phương thức bất đồng bộ để lưu thay đổi vào cơ sở dữ liệu.
        Task SaveAsync();

        // Phương thức bất đồng bộ để lấy tất cả các tiền phạt theo LoanId từ cơ sở dữ liệu.
        Task<IEnumerable<Fine>> GetFinesByLoanIdAsync(int loanId);

        // Phương thức bất đồng bộ để lấy tất cả các tiền phạt chưa thanh toán từ cơ sở dữ liệu.
        Task<IEnumerable<Fine>> GetUnpaidFinesAsync();

        // Kiểm tra đã có phiếu phạt chưa theo LoanId
        Task<bool> HasUnpaidFineByLoanIdAsync(int loanId);
    }
}

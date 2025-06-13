using System.Collections.Generic;  // Cung cấp các kiểu dữ liệu danh sách, ví dụ: IEnumerable.
using System.Threading.Tasks;  // Cung cấp các kiểu bất đồng bộ như Task.
using QuanLyThuVien.Models;  // Cung cấp các mô hình dữ liệu như Loan, v.v.

public interface ILoanRepository
{
    // Phương thức bất đồng bộ để lấy tất cả các khoản vay từ cơ sở dữ liệu.
    Task<IEnumerable<Loan>> GetAllAsync();

    // Phương thức bất đồng bộ để lấy một khoản vay theo ID từ cơ sở dữ liệu.
    Task<Loan?> GetByIdAsync(int id);

    // Phương thức bất đồng bộ để thêm một khoản vay mới vào cơ sở dữ liệu.
    Task AddAsync(Loan loan);

    // Phương thức bất đồng bộ để cập nhật thông tin của một khoản vay trong cơ sở dữ liệu.
    Task UpdateAsync(Loan loan);

    // Phương thức bất đồng bộ để xóa một khoản vay khỏi cơ sở dữ liệu theo ID.
    Task DeleteAsync(int id);

    // Phương thức bất đồng bộ để tính tiền phạt cho một khoản vay dựa trên LoanId.
    Task<decimal> CalculateFineAsync(int loanId);

    // Lấy danh sách các khoản vay quá hạn chưa có phiếu phạt
    Task<IEnumerable<Loan>> GetOverdueLoansWithoutFineAsync();
}

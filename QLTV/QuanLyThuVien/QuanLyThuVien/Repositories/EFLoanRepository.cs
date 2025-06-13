using System;  // Cung cấp các kiểu dữ liệu và phương thức hỗ trợ làm việc với kiểu dữ liệu DateTime, v.v.
using System.Collections.Generic;  // Cung cấp các kiểu dữ liệu Collection như List, IEnumerable, v.v.
using System.Linq;  // Cung cấp các phương thức LINQ để truy vấn dữ liệu.
using System.Threading.Tasks;  // Hỗ trợ các tác vụ bất đồng bộ (asynchronous).
using Microsoft.EntityFrameworkCore;  // Cung cấp các lớp và phương thức hỗ trợ làm việc với Entity Framework (EF) Core.
using QuanLyThuVien.Models;  // Chứa các mô hình dữ liệu trong ứng dụng, bao gồm các lớp như Loan, Book, LibraryCard, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Lớp LoanRepository triển khai ILoanRepository để thực hiện các thao tác với bảng Loans trong cơ sở dữ liệu.
    public class LoanRepository : ILoanRepository
    {
        private readonly HuyDBContext _context;  // Khai báo một biến để lưu trữ ngữ cảnh của cơ sở dữ liệu (HuyDBContext).

        // Constructor nhận đối tượng HuyDBContext để kết nối với cơ sở dữ liệu.
        public LoanRepository(HuyDBContext context)
        {
            _context = context;  // Khởi tạo _context bằng đối tượng context được truyền vào từ bên ngoài.
        }

        // Phương thức lấy tất cả các khoản vay từ cơ sở dữ liệu.
        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            // Lấy tất cả các khoản vay từ bảng Loans, bao gồm thông tin Book và LibraryCard liên quan.
            var loans = await _context.Loans
                .Include(l => l.Book)  // Tải thông tin sách (Book) liên quan.
                .Include(l => l.LibraryCard)  // Tải thông tin thẻ thư viện (LibraryCard) liên quan.
                .ToListAsync();  // Chuyển kết quả thành danh sách và trả về.

            // Cập nhật trạng thái (status) cho mỗi khoản vay.
            foreach (var loan in loans)
            {
                loan.UpdateStatus();
            }

            return loans;  // Trả về danh sách các khoản vay.
        }

        // Phương thức lấy khoản vay theo ID từ cơ sở dữ liệu.
        public async Task<Loan?> GetByIdAsync(int id)
        {
            // Tìm khoản vay với LoanId = id, bao gồm thông tin Book và LibraryCard liên quan.
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.LibraryCard)
                .FirstOrDefaultAsync(l => l.LoanId == id);  // Trả về khoản vay đầu tiên tìm thấy hoặc null nếu không có.

            // Nếu tìm thấy khoản vay, cập nhật trạng thái.
            if (loan != null)
            {
                loan.UpdateStatus();
            }

            return loan;  // Trả về khoản vay tìm thấy hoặc null.
        }

        // Phương thức thêm một khoản vay mới vào cơ sở dữ liệu.
        public async Task AddAsync(Loan loan)
        {
            loan.UpdateStatus();  // Cập nhật trạng thái của khoản vay trước khi thêm.
            await _context.Loans.AddAsync(loan);  // Thêm khoản vay vào bảng Loans.
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu.
        }

        // Phương thức cập nhật thông tin khoản vay trong cơ sở dữ liệu.
        public async Task UpdateAsync(Loan loan)
        {
            loan.UpdateStatus();  // Cập nhật trạng thái của khoản vay trước khi cập nhật.
            _context.Loans.Update(loan);  // Cập nhật thông tin khoản vay trong bảng Loans.
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu.
        }

        // Phương thức xóa khoản vay theo ID từ cơ sở dữ liệu.
        public async Task DeleteAsync(int id)
        {
            // Tìm khoản vay theo ID.
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)  // Nếu tìm thấy khoản vay.
            {
                _context.Loans.Remove(loan);  // Xóa khoản vay khỏi bảng Loans.
                await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu.
            }
        }

        // Phương thức tính toán số tiền phạt của một khoản vay.
        public async Task<decimal> CalculateFineAsync(int loanId)
        {
            // Tìm khoản vay theo LoanId.
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return 0;  // Nếu không tìm thấy khoản vay, trả về 0.
            }

            loan.UpdateStatus();  // Cập nhật trạng thái của khoản vay.

            // Nếu khoản vay không quá hạn, không có tiền phạt.
            if (loan.Status != LoanStatus.Overdue)
            {
                return 0;
            }

            // Tính số ngày quá hạn.
            int daysOverdue = (DateTime.Now - loan.DueDate).Days;
            decimal finePerDay = 2000;  // Tiền phạt mỗi ngày là 2.000 VNĐ.

            return daysOverdue * finePerDay;  // Trả về số tiền phạt.
        }

        // Phương thức lưu các thay đổi vào cơ sở dữ liệu.
        public void Save() => _context.SaveChanges();  // Lưu tất cả thay đổi trong ngữ cảnh _context vào cơ sở dữ liệu.

        // Lấy danh sách các khoản vay quá hạn chưa có phiếu phạt
        public async Task<IEnumerable<Loan>> GetOverdueLoansWithoutFineAsync()
        {
            // Lấy tất cả khoản vay quá hạn
            var overdueLoans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.LibraryCard)
                .Where(l => l.Status == LoanStatus.Overdue)
                .ToListAsync();

            // Lọc ra các khoản vay chưa có phiếu phạt
            var loansWithoutFine = overdueLoans.Where(loan =>
                !_context.Fines.Any(f => f.LoanId == loan.LoanId && f.Status != FineStatus.Paid)
            ).ToList();

            return loansWithoutFine;
        }
    }
}

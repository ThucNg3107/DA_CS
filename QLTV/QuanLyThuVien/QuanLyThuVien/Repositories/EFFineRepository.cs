using Microsoft.EntityFrameworkCore;  // Cung cấp các lớp và phương thức hỗ trợ làm việc với Entity Framework (EF) Core.
using QuanLyThuVien.Models;  // Chứa các mô hình dữ liệu trong ứng dụng, bao gồm các lớp như Fine, Loan, Book, LibraryCard, v.v.

namespace QuanLyThuVien.Repositories  // Định nghĩa không gian tên cho các lớp repository trong ứng dụng quản lý thư viện.
{
    // Lớp EFFineRepository triển khai IFineRepository để thực hiện các thao tác với bảng Fine trong cơ sở dữ liệu.
    public class EFFineRepository : IFineRepository
    {
        private readonly HuyDBContext _context;  // Khai báo một biến để lưu trữ ngữ cảnh của cơ sở dữ liệu (HuyDBContext).

        // Constructor nhận đối tượng HuyDBContext để kết nối với cơ sở dữ liệu.
        public EFFineRepository(HuyDBContext context)
        {
            _context = context;  // Khởi tạo _context bằng đối tượng context được truyền vào từ bên ngoài.
        }

        // Phương thức lấy tất cả các phiếu phạt từ cơ sở dữ liệu.
        public async Task<IEnumerable<Fine>> GetAllAsync()
        {
            // Sử dụng EF Core để lấy tất cả phiếu phạt từ bảng Fines, bao gồm thông tin về Loan, Book, và LibraryCard liên quan.
            return await _context.Fines
                .Include(f => f.Loan)  // Load thông tin Loan liên quan.
                    .ThenInclude(l => l.Book)  // Load thông tin Book liên quan đến Loan.
                .Include(f => f.Loan)  // Load lại thông tin Loan để lấy LibraryCard.
                    .ThenInclude(l => l.LibraryCard)  // Load thông tin LibraryCard liên quan đến Loan.
                .ToListAsync();  // Chuyển kết quả thành danh sách và trả về.
        }

        // Phương thức lấy phiếu phạt theo ID từ cơ sở dữ liệu.
        public async Task<Fine?> GetByIdAsync(int id)
        {
            // Tìm phiếu phạt với FineId = id, bao gồm thông tin về Loan, Book, và LibraryCard liên quan.
            return await _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Book)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.LibraryCard)
                .FirstOrDefaultAsync(f => f.FineId == id);  // Trả về phiếu phạt tìm thấy hoặc null nếu không tìm thấy.
        }

        // Phương thức thêm một phiếu phạt mới vào cơ sở dữ liệu.
        public async Task<Fine> AddAsync(Fine fine)
        {
            // Thêm phiếu phạt vào bảng Fines.
            _context.Fines.Add(fine);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await SaveAsync();
            return fine;  // Trả về phiếu phạt đã được thêm vào cơ sở dữ liệu.
        }

        // Phương thức cập nhật thông tin phiếu phạt trong cơ sở dữ liệu.
        public async Task UpdateAsync(Fine fine)
        {
            // Đánh dấu phiếu phạt là đã sửa đổi.
            _context.Entry(fine).State = EntityState.Modified;
            // Lưu thay đổi vào cơ sở dữ liệu.
            await SaveAsync();
        }

        // Phương thức xóa phiếu phạt khỏi cơ sở dữ liệu.
        public async Task DeleteAsync(Fine fine)
        {
            // Xóa phiếu phạt khỏi bảng Fines.
            _context.Fines.Remove(fine);
            // Lưu thay đổi vào cơ sở dữ liệu.
            await SaveAsync();
        }

        // Phương thức lưu các thay đổi vào cơ sở dữ liệu.
        public async Task SaveAsync()
        {
            // Lưu tất cả thay đổi vào cơ sở dữ liệu.
            await _context.SaveChangesAsync();
        }

        // Phương thức lấy tất cả phiếu phạt theo LoanId.
        public async Task<IEnumerable<Fine>> GetFinesByLoanIdAsync(int loanId)
        {
            // Lấy tất cả phiếu phạt với LoanId tương ứng, bao gồm thông tin Loan, Book, và LibraryCard liên quan.
            return await _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Book)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.LibraryCard)
                .Where(f => f.LoanId == loanId)  // Lọc phiếu phạt theo LoanId.
                .ToListAsync();  // Chuyển kết quả thành danh sách và trả về.
        }

        // Phương thức lấy tất cả phiếu phạt chưa thanh toán.
        public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync()
        {
            // Lấy tất cả phiếu phạt chưa thanh toán, bao gồm thông tin Loan, Book, và LibraryCard liên quan.
            return await _context.Fines
                .Include(f => f.Loan)
                    .ThenInclude(l => l.Book)
                .Include(f => f.Loan)
                    .ThenInclude(l => l.LibraryCard)
                .Where(f => f.Status != FineStatus.Paid)  // Lọc phiếu phạt chưa được thanh toán.
                .ToListAsync();  // Chuyển kết quả thành danh sách và trả về.
        }

        // Kiểm tra đã có phiếu phạt chưa theo LoanId
        public async Task<bool> HasUnpaidFineByLoanIdAsync(int loanId)
        {
            return await _context.Fines.AnyAsync(f => f.LoanId == loanId && f.Status != FineStatus.Paid);
        }
    }
}

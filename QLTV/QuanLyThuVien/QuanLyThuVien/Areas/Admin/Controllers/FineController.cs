using Microsoft.AspNetCore.Authorization; // Dùng cho phân quyền truy cập
using Microsoft.AspNetCore.Mvc; // Dùng cho Controller và các ActionResult
using Microsoft.AspNetCore.Mvc.Rendering; // Dùng để tạo SelectList cho dropdown
using QuanLyThuVien.Areas.Admin.Models; // Models riêng cho khu vực Admin
using QuanLyThuVien.Models; // Models chung toàn hệ thống
using QuanLyThuVien.Repositories; // Giao diện Repository cho truy vấn dữ liệu

namespace QuanLyThuVien.Areas.Admin.Controllers
{
    // Định danh controller nằm trong Area "Admin"
    [Area("Admin")]

    // Chỉ cho phép tài khoản có Role = Admin truy cập controller này
    [Authorize(Roles = SD.Role_Admin)]
    public class FineController : Controller
    {
        // Khai báo các repository để thao tác với dữ liệu
        private readonly IFineRepository _fineRepository;
        private readonly ILoanRepository _loanRepository;

        // Constructor để inject các repository vào controller
        public FineController(IFineRepository fineRepository, ILoanRepository loanRepository)
        {
            _fineRepository = fineRepository;
            _loanRepository = loanRepository;
        }

        // Hiển thị danh sách phiếu phạt
        public async Task<IActionResult> Index()
        {
            var fines = await _fineRepository.GetAllAsync(); // Lấy tất cả phiếu phạt từ database
            return View(fines); // Truyền danh sách này sang view Index
        }

        // Hiển thị chi tiết 1 phiếu phạt
        public async Task<IActionResult> Details(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id); // Lấy phiếu phạt theo id
            if (fine == null)
                return NotFound(); // Trả về 404 nếu không tìm thấy

            return View(fine); // Truyền model sang view
        }

        // GET: Hiển thị form tạo mới phiếu phạt
        public async Task<IActionResult> Create()
        {
            var loans = await _loanRepository.GetAllAsync(); // Lấy tất cả phiếu mượn
            // Tạo dropdown danh sách phiếu mượn
            ViewBag.Loans = new SelectList(loans.Select(l => new
            {
                l.LoanId,
                DisplayText = $"Phiếu mượn #{l.LoanId} - {l.Book?.Name} - {l.LibraryCard?.FullName}"
            }), "LoanId", "DisplayText");

            return View(); // Trả về view Create
        }

        // POST: Xử lý dữ liệu khi submit form tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,Amount,DueDate,Description")] Fine fine)
        {
            if (ModelState.IsValid) // Kiểm tra dữ liệu hợp lệ
            {
                fine.CreatedDate = DateTime.Now; // Gán ngày tạo hiện tại
                fine.Status = FineStatus.Pending; // Trạng thái mặc định: chưa thanh toán
                await _fineRepository.AddAsync(fine); // Lưu phiếu phạt vào DB
                return RedirectToAction(nameof(Index)); // Chuyển về trang danh sách
            }

            // Nếu dữ liệu không hợp lệ, load lại dropdown
            var loans = await _loanRepository.GetAllAsync();
            ViewBag.Loans = new SelectList(loans.Select(l => new
            {
                l.LoanId,
                DisplayText = $"Phiếu mượn #{l.LoanId} - {l.Book?.Name} - {l.LibraryCard?.FullName}"
            }), "LoanId", "DisplayText");

            return View(fine); // Trả lại view với dữ liệu đã nhập
        }

        // GET: Hiển thị form chỉnh sửa phiếu phạt
        public async Task<IActionResult> Edit(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id); // Lấy phiếu phạt theo ID
            if (fine == null)
                return NotFound(); // Trả về 404 nếu không có

            // Tạo dropdown danh sách phiếu mượn
            var loans = await _loanRepository.GetAllAsync();
            ViewBag.Loans = new SelectList(loans.Select(l => new
            {
                l.LoanId,
                DisplayText = $"Phiếu mượn #{l.LoanId} - {l.Book?.Name} - {l.LibraryCard?.FullName}"
            }), "LoanId", "DisplayText");

            return View(fine); // Trả về view Edit
        }

        // POST: Xử lý khi người dùng submit form chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FineId,LoanId,Amount,DueDate,Description")] Fine fine)
        {
            if (id != fine.FineId) // So sánh ID tránh lỗi
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingFine = await _fineRepository.GetByIdAsync(id); // Lấy phiếu phạt gốc
                if (existingFine == null)
                    return NotFound();

                // Cập nhật thông tin
                existingFine.Amount = fine.Amount;
                existingFine.DueDate = fine.DueDate;
                existingFine.Description = fine.Description;

                await _fineRepository.UpdateAsync(existingFine); // Cập nhật vào DB
                return RedirectToAction(nameof(Index)); // Về trang danh sách
            }

            // Nếu dữ liệu không hợp lệ, tạo lại dropdown
            var loans = await _loanRepository.GetAllAsync();
            ViewBag.Loans = new SelectList(loans.Select(l => new
            {
                l.LoanId,
                DisplayText = $"Phiếu mượn #{l.LoanId} - {l.Book?.Name} - {l.LibraryCard?.FullName}"
            }), "LoanId", "DisplayText");

            return View(fine); // Trả lại view với lỗi
        }

        // GET: Hiển thị trang xác nhận xoá
        public async Task<IActionResult> Delete(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id); // Lấy phiếu phạt theo ID
            if (fine == null)
                return NotFound();

            return View(fine); // Trả về view xác nhận xoá
        }

        // POST: Xác nhận xoá thật sự
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id);
            if (fine != null)
            {
                await _fineRepository.DeleteAsync(fine); // Thực hiện xoá trong DB
            }
            return RedirectToAction(nameof(Index)); // Quay lại danh sách
        }

        // POST: Đánh dấu đã thanh toán phiếu phạt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id);
            if (fine != null)
            {
                fine.Status = FineStatus.Paid; // Đổi trạng thái thành đã thanh toán
                fine.PaidDate = DateTime.Now;  // Lưu ngày thanh toán
                await _fineRepository.UpdateAsync(fine); // Cập nhật DB
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Hiển thị các phiếu phạt chưa thanh toán
        public async Task<IActionResult> UnpaidFines()
        {
            var unpaidFines = await _fineRepository.GetUnpaidFinesAsync(); // Lọc theo Status = Pending
            return View("Index", unpaidFines); // Dùng lại view Index
        }

        // POST: Cập nhật trạng thái quá hạn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var fine = await _fineRepository.GetByIdAsync(id);
            if (fine == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật nếu phiếu đang ở trạng thái chờ thanh toán và đã quá hạn
            if (fine.Status == FineStatus.Pending && DateTime.Now > fine.DueDate)
            {
                fine.Status = FineStatus.Overdue;
                await _fineRepository.UpdateAsync(fine);
            }

            return Ok();
        }

        // Action tự động tạo phiếu phạt cho các khoản vay quá hạn chưa có phiếu phạt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoCreateFinesForOverdueLoans()
        {
            var overdueLoans = await _loanRepository.GetOverdueLoansWithoutFineAsync();
            foreach (var loan in overdueLoans)
            {
                int daysOverdue = (DateTime.Now - loan.DueDate).Days;
                if (daysOverdue > 0)
                {
                    var fine = new Fine
                    {
                        LoanId = loan.LoanId,
                        Amount = daysOverdue * 2000,
                        CreatedDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(7),
                        Status = FineStatus.Pending,
                        Description = $"Phạt quá hạn {daysOverdue} ngày x 2.000đ/ngày."
                    };
                    await _fineRepository.AddAsync(fine);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

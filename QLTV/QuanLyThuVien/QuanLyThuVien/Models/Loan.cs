using System;  
using System.ComponentModel.DataAnnotations;  
using System.ComponentModel.DataAnnotations.Schema;  

namespace QuanLyThuVien.Models  // Định nghĩa không gian tên (namespace) cho các mô hình của ứng dụng quản lý thư viện.
{
    public class Loan  // Lớp đại diện cho một khoản vay (mượn sách) trong hệ thống.
    {
        [Key]  // Đánh dấu thuộc tính LoanId là khóa chính trong cơ sở dữ liệu.
        public int LoanId { get; set; }  // Mã số khoản vay (mượn sách).

        [ForeignKey(nameof(Book))]  // Đánh dấu BookId là khóa ngoại liên kết với bảng Books.
        public int BookId { get; set; }  // Mã số sách đã được mượn.

        [ForeignKey(nameof(LibraryCard))]  // Đánh dấu CardId là khóa ngoại liên kết với bảng LibraryCards.
        public int CardId { get; set; }  // Mã thẻ thư viện của người mượn sách.

        [Required]  // Đánh dấu thuộc tính này là bắt buộc.
        [Display(Name = "Ngày mượn")]  // Định nghĩa tên hiển thị trong giao diện người dùng là "Ngày mượn".
        public DateTime BorrowedDate { get; set; } = DateTime.Now;  // Ngày mượn sách, mặc định là thời gian hiện tại.

        [Required]  // Đánh dấu thuộc tính này là bắt buộc.
        [Display(Name = "Hạn trả")]  // Định nghĩa tên hiển thị trong giao diện người dùng là "Hạn trả".
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);  // Ngày phải trả sách, mặc định là 14 ngày sau ngày mượn.

        [Display(Name = "Trạng thái")]  // Định nghĩa tên hiển thị trong giao diện người dùng là "Trạng thái".
        public LoanStatus Status { get; set; } = LoanStatus.Borrowed;  // Trạng thái của khoản vay (mượn sách), mặc định là "Đang mượn".

        [Display(Name = "Tiền ứng trước")]
        [DataType(DataType.Currency)]
        public decimal DepositAmount { get; set; }

        [Display(Name = "Trạng thái mượn")]
        public BorrowStatus BorrowStatus { get; set; } = BorrowStatus.NguyenVen;

        // Các thuộc tính điều hướng (navigation properties) để kết nối với bảng Book và LibraryCard.
        public virtual Book? Book { get; set; }  // Một sách có thể có nhiều khoản vay.
        public virtual LibraryCard? LibraryCard { get; set; }  // Một thẻ thư viện có thể mượn nhiều sách.

        // Phương thức để cập nhật trạng thái khoản vay (mượn sách).
        public void UpdateStatus()
        {
            if (Status == LoanStatus.Returned) return;
            Status = DueDate < DateTime.Now ? LoanStatus.Overdue : LoanStatus.Borrowed;
        }
    }

    // Enum LoanStatus để chỉ ra trạng thái của khoản vay.
    public enum LoanStatus
    {
        Borrowed,  // Đang mượn
        Overdue,   // Quá hạn
        Returned   // Đã trả
    }

    public enum BorrowStatus
    {
        NguyenVen, // Nguyên vẹn
        SuCo       // Sự cố
    }
}

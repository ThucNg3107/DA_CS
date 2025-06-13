using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace QuanLyThuVien.Models // Namespace chứa các model
{
    public class Fine // Lớp mô hình đại diện cho khoản phạt
    {
        [Key] // Đánh dấu FineId là khóa chính trong cơ sở dữ liệu
        public int FineId { get; set; }

        [Required] // Đánh dấu LoanId là trường bắt buộc
        public int LoanId { get; set; } // Mã số của khoản mượn sách

        [Required] // Đánh dấu Amount là trường bắt buộc
        [Column(TypeName = "decimal(18,2)")] // Xác định kiểu dữ liệu là decimal với 18 chữ số và 2 chữ số thập phân
        public decimal Amount { get; set; } // Số tiền phạt

        [Required] // Đánh dấu DueDate là trường bắt buộc
        public DateTime DueDate { get; set; } // Ngày đến hạn trả sách

        [Required] // Đánh dấu CreatedDate là trường bắt buộc
        public DateTime CreatedDate { get; set; } // Ngày tạo khoản phạt

        public DateTime? PaidDate { get; set; } // Ngày thanh toán khoản phạt (nếu có), có thể null

        [Required] // Đánh dấu Status là trường bắt buộc
        public FineStatus Status { get; set; } // Trạng thái của khoản phạt (Chờ thanh toán, Đã thanh toán, Quá hạn)

        [StringLength(500)] // Đặt giới hạn độ dài mô tả khoản phạt là 500 ký tự
        public string? Description { get; set; } // Mô tả chi tiết về khoản phạt (tùy chọn)

        // Navigation property: Mối quan hệ giữa Fine và Loan
        public virtual Loan? Loan { get; set; }
    }

    // Enum cho trạng thái của khoản phạt
    public enum FineStatus
    {
        Pending, // Đang chờ thanh toán
        Paid, // Đã thanh toán
        Overdue // Quá hạn
    }
}

using System.ComponentModel.DataAnnotations;  
using System.ComponentModel.DataAnnotations.Schema;  
using System.Collections.Generic;  

namespace QuanLyThuVien.Models  // Định nghĩa không gian tên (namespace) cho ứng dụng quản lý thư viện
{
    public class LibraryCard  // Lớp đại diện cho thẻ thư viện
    {
        [Key]  // Đánh dấu thuộc tính này là khóa chính (Primary Key) trong cơ sở dữ liệu
        public int CardId { get; set; }  // Mã thẻ thư viện, khóa chính

        [Required]  // Đánh dấu trường này là bắt buộc phải có giá trị
        [Display(Name = "Student ID")]  // Định nghĩa tên hiển thị cho trường này trong giao diện người dùng
        [RegularExpression(@"^\d{10}$", ErrorMessage = "ID đăng ký phải có 10 chữ số")]  // Đặt biểu thức chính quy để kiểm tra định dạng của Mã số sinh viên (phải là chuỗi gồm 10 chữ số)
        public string StudentID { get; set; } = string.Empty;  // Mã số sinh viên của người dùng, phải có đúng 10 chữ số

        [Required]  // Trường này là bắt buộc
        [Display(Name = "Full Name")]  // Định nghĩa tên hiển thị cho trường này trong giao diện người dùng
        public string FullName { get; set; } = string.Empty;  // Tên đầy đủ của người dùng

        [Required]  // Trường này là bắt buộc
        [Display(Name = "Phone Number")]  // Định nghĩa tên hiển thị cho trường này trong giao diện người dùng
        [RegularExpression(@"^(0\d{9,10})$", ErrorMessage = "Số điện thoại không hợp lệ")]  // Đặt biểu thức chính quy để kiểm tra số điện thoại (phải bắt đầu bằng "0" và có từ 10 đến 11 chữ số)
        public string PhoneNumber { get; set; } = string.Empty;  // Số điện thoại của người dùng, phải hợp lệ theo biểu thức chính quy

        [Required]  // Trường này là bắt buộc
        [Display(Name = "Issued Date")]  // Định nghĩa tên hiển thị cho trường này trong giao diện người dùng
        public DateTime IssuedDate { get; set; } = DateTime.Now;  // Ngày cấp thẻ thư viện, mặc định là thời gian hiện tại khi tạo thẻ

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        // Khóa ngoại đến bảng AspNetUsers (HuyUser)
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public HuyUser? User { get; set; }

        public virtual ICollection<Loan>? Loans { get; set; }
    }
}

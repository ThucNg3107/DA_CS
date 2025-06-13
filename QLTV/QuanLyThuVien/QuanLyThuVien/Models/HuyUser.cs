using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using System.ComponentModel.DataAnnotations;  
using Microsoft.AspNetCore.Identity;  

namespace QuanLyThuVien.Models  // Định nghĩa không gian tên cho mô hình của ứng dụng quản lý thư viện
{
    // Lớp HuyUser kế thừa IdentityUser để mở rộng thông tin người dùng
    public class HuyUser : IdentityUser
    {
        // Thuộc tính FullName đại diện cho tên đầy đủ của người dùng.
        // Annotation [Required] đánh dấu trường này là bắt buộc.
        [Required]
        public string FullName { get; set; }  // Tên đầy đủ của người dùng

        // Thuộc tính Address lưu trữ địa chỉ của người dùng. Trường này có thể null.
        public string? Address { get; set; }  // Địa chỉ của người dùng (có thể bỏ trống)

        // Thuộc tính Age lưu trữ độ tuổi của người dùng. Trường này có thể null.
        public string? Age { get; set; }  // Độ tuổi của người dùng (có thể bỏ trống)
    }
}

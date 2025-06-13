using System.ComponentModel.DataAnnotations; 

namespace QuanLyThuVien.Models // Namespace chứa các model
{
    public class Category // Lớp mô hình cho thể loại sách
    {
        [Key] // Đánh dấu CategoryId là khóa chính trong cơ sở dữ liệu
        public int CategoryId { get; set; }

        [Required] // Đánh dấu Name là trường bắt buộc
        public string Name { get; set; } = string.Empty; // Tên của thể loại, mặc định là chuỗi rỗng

        // Sửa lỗi null khi không có sách nào thuộc danh mục này
        public virtual List<Book> Books { get; set; } = new(); // Danh sách các cuốn sách thuộc thể loại này, mặc định là một danh sách rỗng
    }
}

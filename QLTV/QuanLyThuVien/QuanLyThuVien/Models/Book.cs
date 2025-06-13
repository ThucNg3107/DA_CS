using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace QuanLyThuVien.Models // Namespace chứa các model
{
    public class Book // Lớp mô hình cho một cuốn sách
    {
        // Enum cho trạng thái của sách
        public enum Status
        {
            Available, // Sách có sẵn
            Borrowed, // Sách đã được mượn
            Lost // Sách bị mất
        }

        [Key] // Đánh dấu BookId là khóa chính trong cơ sở dữ liệu
        public int BookId { get; set; }

        [Required] // Đánh dấu Name là trường bắt buộc
        public string Name { get; set; } = string.Empty; // Tên sách, mặc định là chuỗi rỗng

        [ForeignKey(nameof(Category))] // Đánh dấu CategoryId là khóa ngoại trỏ đến bảng Category
        public int CategoryId { get; set; }

        [Required] // Đánh dấu Author là trường bắt buộc
        public string Author { get; set; } = string.Empty; // Tên tác giả

        [Required] // Đánh dấu Publisher là trường bắt buộc
        public string Publisher { get; set; } = string.Empty; // Nhà xuất bản

        [Range(1000, 2100, ErrorMessage = "Năm xuất bản không hợp lệ")] // Kiểm tra năm xuất bản trong khoảng từ 1000 đến 2100
        public int PublishedYear { get; set; } // Năm xuất bản của sách

        public Status BookStatus { get; set; } = Status.Available; // Trạng thái của sách, mặc định là Available (Có sẵn)

        public string? ImageUrl { get; set; } // Đường dẫn đến hình ảnh của sách (tùy chọn)

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sách phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; } = 0; // Số lượng sách hiện có

        [Required]
        public int ShelfRow { get; set; } // Dãy kệ (ví dụ: dãy 3)

        [Required]
        public int ShelfNumber { get; set; } // Số kệ (ví dụ: kệ 3)

        public virtual Category? Category { get; set; } // Mối quan hệ với bảng Category (nếu có) để hiển thị tên thể loại sách
    }
}

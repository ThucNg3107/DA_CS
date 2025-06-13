using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity; 

namespace QuanLyThuVien.Models // Namespace chứa mô hình và cấu hình cho cơ sở dữ liệu
{
    // Lớp HuyDBContext kế thừa từ IdentityDbContext, giúp quản lý dữ liệu người dùng và các bảng liên quan
    public class HuyDBContext : IdentityDbContext<HuyUser>
    {
        // Constructor nhận DbContextOptions để cấu hình kết nối cơ sở dữ liệu
        public HuyDBContext(DbContextOptions<HuyDBContext> options) : base(options) { }

        // Các DbSet đại diện cho các bảng trong cơ sở dữ liệu
        public DbSet<Book> Books { get; set; } // Bảng Books chứa các sách
        public DbSet<Category> Categories { get; set; } // Bảng Categories chứa các danh mục sách
        public DbSet<LibraryCard> LibraryCards { get; set; } // Bảng LibraryCards chứa thông tin thẻ thư viện
        public DbSet<Loan> Loans { get; set; } // Bảng Loans chứa thông tin mượn sách
        public DbSet<Fine> Fines { get; set; } // Bảng Fines chứa các khoản phạt khi trả sách trễ
        public DbSet<Comment> Comments { get; set; } // Bảng Comments chứa bình luận và đánh giá sách
    }
}

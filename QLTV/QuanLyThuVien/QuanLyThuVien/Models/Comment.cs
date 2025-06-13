using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        public string? UserId { get; set; } // Nếu muốn liên kết với người dùng

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Range(1,5)]
        public int Rating { get; set; } // Đánh giá 1-5 sao

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 
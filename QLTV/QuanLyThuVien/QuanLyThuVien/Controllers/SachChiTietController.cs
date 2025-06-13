using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;
using QuanLyThuVien.Repositories;
using Microsoft.AspNetCore.Identity;

public class SachChiTietController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly HuyDBContext _context;
    private readonly UserManager<HuyUser> _userManager;

    public SachChiTietController(IBookRepository bookRepository, ICategoryRepository categoryRepository, HuyDBContext context, UserManager<HuyUser> userManager)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _context = context;
        _userManager = userManager;
    }

    // Action Index để hiển thị danh sách sách và hỗ trợ tìm kiếm
    public async Task<IActionResult> Index(int? categoryId)
    {
        // Lấy tất cả sách từ repository
        var allBooks = await _bookRepository.GetAllAsync();
        var books = allBooks.AsQueryable();

        // Lọc theo category nếu được chọn
        if (categoryId.HasValue)
        {
            books = books.Where(b => b.CategoryId == categoryId.Value);
            ViewData["CategoryId"] = categoryId;
        }

        // Lấy danh sách thể loại để hiển thị trên dropdown tìm kiếm
        var categories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = categories;

        // Trả về danh sách sách dưới dạng danh sách (List)
        return View(books.ToList());
    }

    // Action để xem chi tiết sách
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        // Load Category cho sách nếu có CategoryId
        if (book.CategoryId > 0)
        {
            book.Category = await _categoryRepository.GetByIdAsync(book.CategoryId);
        }

        // Lấy bình luận cho sách
        var comments = _context.Comments.Where(c => c.BookId == id).OrderByDescending(c => c.CreatedAt).ToList();
        ViewBag.Comments = comments;

        // Lấy email người dùng đã đăng nhập (nếu có)
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserEmail = user?.Email;
        }
        else
        {
            ViewBag.CurrentUserEmail = null;
        }

        // Lấy email cho từng bình luận
        var userIds = comments.Where(c => !string.IsNullOrEmpty(c.UserId)).Select(c => c.UserId).Distinct().ToList();
        var userEmails = _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionary(u => u.Id, u => u.Email);
        ViewBag.UserEmails = userEmails;

        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int bookId, string content, int rating)
    {
        var userId = _userManager.GetUserId(User);
        var comment = new Comment
        {
            BookId = bookId,
            Content = content,
            Rating = rating,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = bookId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(int commentId, int bookId)
    {
        var userId = _userManager.GetUserId(User);
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return Forbid();
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = bookId });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(int commentId, int bookId, string content, int rating)
    {
        var userId = _userManager.GetUserId(User);
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null || comment.UserId != userId)
            return Forbid();
        comment.Content = content;
        comment.Rating = rating;
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = bookId });
    }
}
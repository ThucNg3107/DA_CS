using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Models;
using QuanLyThuVien.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QuanLyThuVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StatisticalController : Controller
    {
        private readonly HuyDBContext _context;
        private readonly UserManager<HuyUser> _userManager;

        public StatisticalController(HuyDBContext context, UserManager<HuyUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Tổng số người dùng
            var totalUsers = await _userManager.Users.CountAsync();

            // Tổng số sách
            var totalBooks = await _context.Books.CountAsync();

            // Số lượt mượn trong tháng này
            var now = DateTime.Now;
            var totalLoansThisMonth = await _context.Loans
                .Where(l => l.BorrowedDate.Month == now.Month && l.BorrowedDate.Year == now.Year)
                .CountAsync();

            // Thống kê mượn sách theo 6 tháng gần nhất
            var loanStats = await _context.Loans
                .GroupBy(l => new { l.BorrowedDate.Year, l.BorrowedDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .Take(6)
                .ToListAsync();

            loanStats = loanStats.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList(); // Đảm bảo thứ tự tăng dần

            // Thể loại sách phổ biến
            var popularCategories = await _context.Books
                .GroupBy(b => b.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(4)
                .ToListAsync();

            // Bảng mượn sách gần đây
            var recentLoans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.LibraryCard)
                .OrderByDescending(l => l.BorrowedDate)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalBooks = totalBooks;
            ViewBag.TotalLoansThisMonth = totalLoansThisMonth;
            ViewBag.LoanStats = loanStats;
            ViewBag.PopularCategories = popularCategories;
            ViewBag.RecentLoans = recentLoans;

            return View();
        }
    }
} 
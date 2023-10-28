using ExpenseTrackerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Charts;

namespace ExpenseTrackerMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //Last 7 Days
            DateTime StartData = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
           
            List<Transaction> selectedTransaction = await _context.Transaction.Include(x=>x.Category)
                .Where(y => y.Date >= StartData && y.Date <= EndDate).ToListAsync();

            int totalIncome = selectedTransaction.Where(x=>x.Category.Type=="Income").Sum(x=>x.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0");
            int totalExpense = selectedTransaction.Where(x => x.Category.Type == "Expense").Sum(x => x.Amount);

            ViewBag.TotalExpense = totalExpense.ToString("C0");
            //Balance
            int balance = totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0");
            //Doughnout chart - Expense by category
            ViewBag.PieData = selectedTransaction.Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon+" "+k.First().Category.Title,
                    amount = k.Sum(x => x.Amount),
                    formattedAmount = k.Sum(x => x.Amount).ToString("C0")
                }).OrderByDescending(x=>x.amount).ToList();
            //Spline chart Income vx Expense
            //Income
            List<SplineChartData> incomeSummary = selectedTransaction
                .Where(x => x.Category.Type == "Income").GroupBy(x=>x.Date)
                .Select(x => new SplineChartData { day = x.First().Date.ToString("dd-MMM"), income = x.Sum(y=>y.Amount)}).ToList();

            //Expense
            List<SplineChartData> expenseSummary = selectedTransaction
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();
            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => StartData.AddDays(i).ToString("dd-MMM"))
                .ToArray();
            ViewBag.SplineChartData = from day in last7Days
                                      join income in incomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in expenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            //Recent transactions
            ViewBag.RecentTransactions = await _context.Transaction.Include(x => x.Category)
                .OrderByDescending(y => y.Date).Take(5).ToListAsync();
            return View();
        }
    }

    public class SplineChartData
    {
        public string day { get; set; }
        public int income { get; set; }
        public int expense { get; set; }

    }
}

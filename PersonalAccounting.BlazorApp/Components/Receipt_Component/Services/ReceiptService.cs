using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.BlazorApp.Components.Receipt_Component.Services
{
    public class ReceiptReportItem
    {
        public int ReceiptId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string MerchantName { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public string PaidByUserName { get; set; }
        public int NumberOfSharees { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal UserPaid { get; set; }
        public decimal AdditionDeduction { get; set; }
        public decimal UserOwed { get; set; }
        public decimal Balance { get; set; }
        public decimal RunningTotal { get; set; } // New property for running total
        public List<ItemReportItem> Items { get; set; }
    }

    public class ItemReportItem
    {
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UserShare { get; set; }
    }

    public class ReceiptService
    {
        private readonly ApplicationDbContext _context; // Replace YourDbContext with your actual DbContext

        public ReceiptService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllReceipts(string userName)
        {
            IQueryable<Receipt> query = _context.Receipts;
            if (userName != null)
            {
                query = query.Where(r => r.PaidByUserName == userName || r.Items.Any(ri => ri.Shares.Any(s => s.UserName == userName)));
            }

            var result = await query.Include(r => r.Items).ThenInclude(r => r.Shares).AsNoTracking().ToListAsync();

            foreach (var item in result)
            {
                item.UserShares = item.CalculateSharedAmounts();
            }

            return result;
        }

        public async Task UpdateAdditionDeduction()
        {
            var receipts = await _context.Receipts.Include(x => x.Items).ThenInclude(i => i.Shares).ToListAsync();
            foreach (var receipt in receipts)
            {
                receipt.AdditionDeduction = receipt.TotalAmount - receipt.Items.Sum(x => x.TotalPrice);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReceiptReportItem>> GenerateReportAsync(string userName, DateTime startDate, DateTime endDate)
        {
            var relatdReceipts = await _context.Receipts
            .Where(r =>
                (r.CreateUserName == userName || r.PaidByUserName== userName
                || r.Items.Any(ri => ri.Shares.Any(s => s.UserName == userName))) && r.Date >= startDate && r.Date <= endDate
            )
            .Include(r => r.Items)
                .ThenInclude(ri => ri.Shares)
            .ToListAsync();

            var reportData = relatdReceipts
            .Select(r => new
            {
                ReceiptId = r.Id,
                ReceiptDate = r.Date.Date + r.Time,
                PaidByUserName = r.PaidByUserName,
                TotalReceiptAmount = r.TotalAmount, // Total of the receipt
                UserPaid = r.PaidByUserName == userName ? r.TotalAmount : 0,
                AdditionDeduction = r.AdditionDeduction,
                MerchantName = r.MerchantName,
                ShopName = r.ShopName,
                NumberOfSharees = r.Items.SelectMany(i => i.Shares.Select(x => x.UserName)).Distinct().Count(),
                Items = r.Items.Select(ri => new
                {
                    Description = ri.Description,
                    TotalPrice = ri.TotalPrice,
                    UserShare = ri.Shares.Where(s => s.UserName == userName).Sum(s => ri.TotalPrice * (s.Share / 100))
                }).ToList()
            })
            .ToList();

            var reportItems = reportData.Select(rd => new ReceiptReportItem
            {
                ReceiptId = rd.ReceiptId,
                MerchantName = rd.MerchantName,
                ShopName = rd.ShopName,
                ReceiptDate = rd.ReceiptDate,
                PaidByUserName = rd.PaidByUserName,
                NumberOfSharees = rd.NumberOfSharees,
                TotalReceiptAmount = rd.TotalReceiptAmount,
                AdditionDeduction = rd.AdditionDeduction,
                UserPaid = rd.UserPaid,
                UserOwed = rd.Items.Sum(i => i.UserShare),
                Balance = rd.UserPaid - rd.Items.Sum(i => i.UserShare),
                Items = rd.Items.Select(i => new ItemReportItem
                {
                    Description = i.Description,
                    TotalPrice = i.TotalPrice,
                    UserShare = i.UserShare
                }).ToList()
            }).OrderBy(x => x.ReceiptDate).ToList();



            decimal runningTotal = 0; // Initialize running total


            foreach (var receipt in reportItems)
            {
                int numberOfUsers = receipt.NumberOfSharees;
                decimal sharedAdditionDeductionPerUser = numberOfUsers > 0 ? receipt.AdditionDeduction / numberOfUsers : 0;
                receipt.UserOwed = receipt.Items.Sum(i => i.UserShare) + sharedAdditionDeductionPerUser;
                receipt.Balance = receipt.UserPaid - receipt.UserOwed;
                runningTotal += receipt.Balance;
                receipt.RunningTotal = runningTotal;
            }

            return reportItems;
        }

        public async Task<Receipt> GetReceiptById(int id)
        {
            return await _context.Receipts.Include(r => r.Items).ThenInclude(r => r.Shares).FirstAsync(r => r.Id == id);
        }

        public async Task AddReceipt(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            FillReceiptItemShares(receipt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReceipt(Receipt receipt)
        {
            _context.Entry(receipt).State = EntityState.Modified;
            FillReceiptItemShares(receipt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReceipt(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
                await _context.SaveChangesAsync();
            }
        }

        private void FillReceiptItemShares(Receipt receipt)
        {
            foreach (var item in receipt.Items)
            {
                if (item.Shares.Count == 0)
                {
                    item.Shares.Add(new ReceiptItemShare
                    {
                        ReceiptItem = item,
                        Share = 100,
                        UserId = receipt.PaidByUserId,
                        UserName = receipt.PaidByUserName,
                        UserFullName = receipt.PaidByUserFullName
                    });
                }
            }
        }
    }
}

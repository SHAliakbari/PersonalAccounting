using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.BlazorApp.Components.Receipt_Component.Services
{
    public class ReceiptService
    {
        private readonly ApplicationDbContext _context; // Replace YourDbContext with your actual DbContext

        public ReceiptService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllReceipts()
        {
            var result = await _context.Receipts.Include(r => r.Items).ThenInclude(r => r.Shares).AsNoTracking().ToListAsync();

            foreach (var item in result)
            {
                item.UserShares = item.CalculateSharedAmounts();
            }

            return result;
        }

        public async Task<Receipt> GetReceiptById(int id)
        {
            return await _context.Receipts.Include(r => r.Items).ThenInclude(r=> r.Shares).FirstAsync(r => r.Id == id);
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

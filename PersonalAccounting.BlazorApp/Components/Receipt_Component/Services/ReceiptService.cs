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
            return await _context.Receipts.Include(r => r.Items).AsNoTracking().ToListAsync();
        }

        public async Task<Receipt> GetReceiptById(int id)
        {
            return await _context.Receipts.Include(r => r.Items).FirstAsync(r => r.Id == id);
        }

        public async Task AddReceipt(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReceipt(Receipt receipt)
        {
            _context.Entry(receipt).State = EntityState.Modified;
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
    }
}

﻿using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Domain.Data;

namespace PersonalAccounting.BlazorApp.Components.NS_TransferRequest.Services
{
    public class TransferRequestService
    {
        private readonly ApplicationDbContext _context;

        public TransferRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransferRequest>> GetTransferRequestsAsync()
        {
            // Eager loading to include details for initial display
            return await _context.TransferRequests
                .Include(m => m.Details)
                .ToListAsync();
        }

        public async Task<TransferRequest?> GetTransferRequestByIdAsync(int id)
        {
            // Eager loading to include details for editing
            return await _context.TransferRequests
                .Include(m => m.Details)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddTransferRequestAsync(TransferRequest TransferRequest)
        {
            _context.TransferRequests.Add(TransferRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransferRequestAsync(TransferRequest TransferRequest)
        {
            // Handle potential concurrency conflicts (optional)
            try
            {
                _context.TransferRequests.Update(TransferRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferRequestExists(TransferRequest.Id))
                {
                    throw new Exception("TransferRequest not found");
                }
                else
                {
                    throw; // Re-throw the concurrency exception
                }
            }
        }

        public async Task DeleteTransferRequestAsync(int id)
        {
            var TransferRequest = await GetTransferRequestByIdAsync(id);
            if (TransferRequest != null)
            {
                _context.TransferRequests.Remove(TransferRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddDetailAsync(TransferRequestDetail detail)
        {
            _context.TransferRequestDetails.Add(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetailAsync(TransferRequestDetail detail)
        {
            // Handle potential concurrency conflicts (optional) - similar to UpdateTransferRequestAsync
            try
            {
                _context.TransferRequestDetails.Update(detail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailExists(detail.Id))
                {
                    throw new Exception("Detail not found");
                }
                else
                {
                    throw; // Re-throw the concurrency exception
                }
            }
        }

        public async Task DeleteDetailAsync(int id)
        {
            var detail = await _context.TransferRequestDetails.FindAsync(id);
            if (detail != null)
            {
                _context.TransferRequestDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        private bool TransferRequestExists(int id)
        {
            return _context.TransferRequests.Any(e => e.Id == id);
        }

        private bool DetailExists(int id)
        {
            return _context.TransferRequestDetails.Any(e => e.Id == id);
        }
    }

}
using _005_Expense_Tracker_API_01.Data;
using _005_Expense_Tracker_API_01.DTOs;
using _005_Expense_Tracker_API_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _005_Expense_Tracker_API_01.Services.Impl
{
    public class TransactionService : ITransactionsService
    {
        private readonly AppDbContext _dbContext;
        
        public TransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResponse<Transaction>> GetAll(GetTransactionQuery query)
        {
            IQueryable<Transaction> transactionsQuery = _dbContext.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                string type = query.Type!.ToLower().Trim();

                transactionsQuery = transactionsQuery.Where(transaction =>
                    transaction.Type.ToLower().Contains(type)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                string category = query.Category!.ToLower().Trim();

                transactionsQuery = transactionsQuery.Where(transaction =>
                    transaction.Category.ToLower().Contains(category)
                );
            }

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                string keyword = query.Search.ToLower().Trim();

                transactionsQuery = transactionsQuery.Where(transaction =>
                    transaction.Title.ToLower().Contains(keyword)
                );
            }

            transactionsQuery = query.Sort.ToLower().Trim() switch
            {
                "asc" => transactionsQuery.OrderBy(transaction => transaction.CreatedAt),
                _ => transactionsQuery.OrderByDescending(transactions => transactions.CreatedAt)
            };

            int totalItems = await transactionsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            List<Transaction> transactions = await transactionsQuery
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

            PaginatedResponse<Transaction> response = new PaginatedResponse<Transaction>
            {
                Items = transactions,
                Page = query.Page,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = query.PageSize
            };

            return response;
        }

        public async Task<Transaction?> GetById(int id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);
        }

        public async Task<Transaction> Create(CreateTransactionRequest request)
        {
            Transaction transaction = new Transaction
            {
                Title = request.Title.Trim(),
                Amount = request.Amount,
                Type = request.Type.Trim(),
                Category = request.Category.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction?> Update(int id, UpdateTransactionRequest request)
        {
            Transaction? transaction = await _dbContext.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);

            if(transaction == null)
            {
                return null;
            }

            transaction.Title = request.Title.Trim();
            transaction.Amount = request.Amount;
            transaction.Category = request.Category.Trim();
            transaction.Type = request.Type.Trim();
            transaction.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public async Task<bool> Delete(int id)
        {
            Transaction? transaction = await _dbContext.Transactions.FirstOrDefaultAsync(transaction => transaction.Id == id);
            if (transaction == null)
            {
                return false;
            }

            _dbContext.Remove(transaction);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<SummaryResponse> GetSummary()
        {
            IQueryable<Transaction> transactionsQuery = _dbContext.Transactions.AsQueryable();

            decimal totalExpense = await transactionsQuery
                .Where(transaction =>
                    transaction.Type == "expense"
                )
                .SumAsync(transaction => transaction.Amount);

            decimal totalIncome = await transactionsQuery
                .Where(transaction =>
                    transaction.Type == "income"
                )
                .SumAsync(transaction => transaction.Amount);

            decimal totalBalance = totalIncome - totalExpense;

            SummaryResponse response = new SummaryResponse
            {
                TotalIncome = totalIncome, 
                TotalExpense = totalExpense,
                TotalBalance = totalBalance,
            };

            return response;
        }
    }
}

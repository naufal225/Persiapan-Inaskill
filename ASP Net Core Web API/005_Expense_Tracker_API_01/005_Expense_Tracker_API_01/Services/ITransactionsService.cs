using _005_Expense_Tracker_API_01.DTOs;
using _005_Expense_Tracker_API_01.Models;

namespace _005_Expense_Tracker_API_01.Services
{
    public interface ITransactionsService
    {
        Task<PaginatedResponse<Transaction>> GetAll(GetTransactionQuery query);
        Task<Transaction?> GetById(int id);
        Task<Transaction> Create(CreateTransactionRequest request);
        Task<Transaction?> Update(int id, UpdateTransactionRequest request);
        Task<bool> Delete(int id);
        Task<SummaryResponse> GetSummary();
    }
}

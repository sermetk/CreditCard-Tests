using CreditCards.Core.Models;
using System.Threading.Tasks;

namespace CreditCards.Core.Interfaces
{
    public interface ICreditCardApplicationRepository
    {
        Task AddAsync(CreditCardApplication application);
    }
}

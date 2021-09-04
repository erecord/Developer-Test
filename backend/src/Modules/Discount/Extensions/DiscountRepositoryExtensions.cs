using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Extensions
{
    public static class DiscountRepositoryExtensions
    {
        public async static Task<Discount> QueryByDiscountCode(this IDiscountRepository repository, string discountCode)
         => (await repository.WhereAsync(d => d.Code.Equals(discountCode))).FirstOrDefault();

    }
}
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Trixx.Common.Utils
{
    public class ManyToManyDbMerger<T12>(DbContext dbContext)
        where T12 : class
    {
        private readonly DbSet<T12> _query = dbContext.Set<T12>();

        public async Task MergeAsync<T2>(
            List<T2> source,
            Expression<Func<T12, bool>> toRemoveFilter,
            Func<T12, T2, bool> selector2,
            Func<T2, T12> generator
            )
        {
            var toRemove = await _query.Where(toRemoveFilter).ToListAsync();

            foreach (var item in source)
            {
                var oldItem = toRemove.FirstOrDefault(x => selector2(x, item));
                if (oldItem is null)
                {
                    _query.Add(generator(item));
                }
                else
                {
                    toRemove.Remove(oldItem);
                }
            }

            _query.RemoveRange(toRemove);
        }
    }
}

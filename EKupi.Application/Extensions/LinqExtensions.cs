using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TSource> SortBy<TSource, TKey>(
            this IQueryable<TSource> source,
            bool isAscending,
            Expression<Func<TSource, TKey>> keySelector
        )
        {
            if (isAscending)
            {
                return source.OrderBy(keySelector);
            }
            else
            {
                return source.OrderByDescending(keySelector);
            }
        }
    }
}

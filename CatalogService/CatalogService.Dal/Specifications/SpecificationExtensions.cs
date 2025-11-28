using Ardalis.Specification;
using System;
using System.Linq.Expressions;

namespace CatalogService.Dal.Specifications
{
    public static class SpecificationExtensions
    {
        public static IOrderedSpecificationBuilder<T> OrderByDynamic<T>(
            this ISpecificationBuilder<T> builder,
            Expression<Func<T, object?>> expr,
            string direction)
        {
            return direction.ToLower() == "desc"
                ? builder.OrderByDescending(expr)
                : builder.OrderBy(expr);
        }

    }
}

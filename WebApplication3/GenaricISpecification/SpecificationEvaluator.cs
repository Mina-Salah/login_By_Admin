using Microsoft.EntityFrameworkCore;

namespace WebApplication3.GenaricISpecification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            foreach (var include in spec.Includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}

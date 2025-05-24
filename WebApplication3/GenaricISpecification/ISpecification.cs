using System.Linq.Expressions;

namespace WebApplication3.GenaricISpecification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
    }
}

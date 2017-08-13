using System.Linq;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    internal class ExpressionTreeModifier : ExpressionVisitor
    {
        private readonly IQueryable<Place> _queryablePlaces;

        internal ExpressionTreeModifier(IQueryable<Place> places)
        {
            _queryablePlaces = places;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            // replace the constant QueryableTerraServerDat arg with the queryable Place collection.
            return constantExpression.Type == typeof(QueryableTerraServerData<Place>) 
                ? Expression.Constant(_queryablePlaces) 
                : constantExpression;
        }
    }
}

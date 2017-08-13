using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    public class QueryableTerraServerData<TData> : IOrderedQueryable<TData>
    {
        public IQueryProvider Provider { get; }
        public Expression Expression { get; }

        public QueryableTerraServerData()
        {
            Provider = new TerraServerQueryProvider();
            Expression = Expression.Constant(this);
        }

        public QueryableTerraServerData(TerraServerQueryProvider provider, Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (!typeof(IQueryable<TData>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException(nameof(expression));

            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression;
        }

        public Type ElementType => typeof(TData);

        public IEnumerator<TData> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<TData>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }
    }
}

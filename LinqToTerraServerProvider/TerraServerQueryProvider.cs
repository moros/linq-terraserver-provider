using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqToTerraServerProvider
{
    public class TerraServerQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable) Activator.CreateInstance(
                    typeof(QueryableTerraServerData<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException ?? exception;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new QueryableTerraServerData<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return TerraServerQueryContext.Execute(expression, false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var isenumerable = (typeof(TResult).Name == "IEnumerable`1");
            return (TResult) TerraServerQueryContext.Execute(expression, isenumerable);
        }
    }
}

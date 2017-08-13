using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    internal class InnermostWhereExpressionVisitor : ExpressionVisitor
    {
        private MethodCallExpression _whereExpression;

        public MethodCallExpression GetInnermostWhere(Expression expression)
        {
            Visit(expression);
            return _whereExpression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.Name == "Where")
                _whereExpression = expression;

            Visit(expression.Arguments[0]);

            return expression;
        }
    }
}

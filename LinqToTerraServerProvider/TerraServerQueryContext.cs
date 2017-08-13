using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    class TerraServerQueryContext
    {
        internal static object Execute(Expression expression, bool isEnumerable)
        {
            // the expression must represent a query over the data source.
            if (!IsQueryOverDataSource(expression))
                throw new InvalidProgramException("No query over the data source was specified.");

            // find the call to where() and get the lambda expression predicate.
            var whereVisitor = new InnermostWhereExpressionVisitor();
            var whereExpression = whereVisitor.GetInnermostWhere(expression);
            var lambdaExpression = (LambdaExpression) ((UnaryExpression) (whereExpression.Arguments[1])).Operand;

            // send the lambda expression through the partial evaluator.
            lambdaExpression = (LambdaExpression) Evaluator.PartialEval(lambdaExpression);

            // get the place name(s) to query the web service with.
            var locationVisitor = new LocationVisitor(lambdaExpression.Body);
            var locations = locationVisitor.Locations;
            if (locations.Count == 0)
                throw new InvalidQueryException("You must specify at least one place name in your query.");

            // call the web service and get the results.
            var places = WebServiceHelper.GetPlacesFromTerraServer(locations);

            // copy the IEnumerable places to an IQueryable.
            var queryablePlaces = places.AsQueryable();

            // copy the expression tree that was passed in, changing only the first
            // argument of the innermost MethodCallExpression.
            var treeCopier = new ExpressionTreeModifier(queryablePlaces);
            var newExpressionTree = treeCopier.Visit(expression);

            // this step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
            return isEnumerable 
                ? queryablePlaces.Provider.CreateQuery(newExpressionTree) 
                : queryablePlaces.Provider.Execute(newExpressionTree);
        }

        private static bool IsQueryOverDataSource(Expression expression)
        {
            // If expression represents an unqueried IQueryable data source instance,
            // expression is of type ConstantExpression, not MethodCallExpression.
            return expression is MethodCallExpression;
        }
    }
}

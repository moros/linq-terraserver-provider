using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    internal class LocationVisitor : ExpressionVisitor
    {
        private readonly Expression _expression;
        private List<string> _locations;

        public LocationVisitor(Expression expression)
        {
            _expression = expression;
        }

        public List<string> Locations
        {
            get
            {
                if (_locations != null)
                    return _locations;

                _locations = new List<string>();
                Visit(_expression);

                return _locations;
            }
        }

        protected override Expression VisitBinary(BinaryExpression exp)
        {
            if (exp.NodeType == ExpressionType.Equal)
            {
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(exp, typeof(Place), "Name"))
                {
                    _locations.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression(exp, typeof(Place), "Name"));
                    return exp;
                }
                else if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(exp, typeof(Place), "State"))
                {
                    _locations.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression(exp, typeof(Place), "State"));
                    return exp;
                }
            }

            return base.VisitBinary(exp);
        }
    }
}

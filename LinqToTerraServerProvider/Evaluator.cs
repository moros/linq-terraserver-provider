using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    public static class Evaluator
    {
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="canBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> canBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(canBeEvaluated).Nominate(expression)).Eval(expression);
        }

        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        /// Evaluates & replaces sub-trees when first condidate is reached (top-down).
        /// </summary>
        class SubtreeEvaluator : ExpressionVisitor
        {
            private readonly HashSet<Expression> _candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                _candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return Visit(exp);
            }

            public override Expression Visit(Expression expression)
            {
                if (expression == null)
                    return null;

                if (_candidates.Contains(expression))
                    return Evaluate(expression);

                return base.Visit(expression);
            }

            private Expression Evaluate(Expression expression)
            {
                if (expression.NodeType == ExpressionType.Constant)
                    return expression;

                var lambda = Expression.Lambda(expression);
                var func = lambda.Compile();

                return Expression.Constant(func.DynamicInvoke(null), expression.Type);
            }
        }

        class Nominator : ExpressionVisitor
        {
            private readonly Func<Expression, bool> _canBeEvaluated;
            private HashSet<Expression> _candidates;
            private bool _cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> canBeEvaluated)
            {
                _canBeEvaluated = canBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                _candidates = new HashSet<Expression>();
                Visit(expression);

                return _candidates;
            }

            public override Expression Visit(Expression expression)
            {
                if (expression == null)
                    return null;

                var saveCannotBeEvaluated = _cannotBeEvaluated;
                _cannotBeEvaluated = false;

                base.Visit(expression);

                if (!_cannotBeEvaluated)
                {
                    if (_canBeEvaluated(expression))
                        _candidates.Add(expression);
                    else
                        _cannotBeEvaluated = true;
                }

                _cannotBeEvaluated |= saveCannotBeEvaluated;

                return expression;
            }
        }
    }
}

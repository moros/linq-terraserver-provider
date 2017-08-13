using System;
using System.Linq.Expressions;

namespace LinqToTerraServerProvider
{
    internal static class ExpressionTreeHelpers
    {
        internal static bool IsMemberEqualsValueExpression(Expression exp, Type declaringType, string memberName)
        {
            if (exp.NodeType != ExpressionType.Equal)
                return false;

            var be = (BinaryExpression) exp;

            // assert
            if (IsSpecificMemberExpression(be.Left, declaringType, memberName)
                && IsSpecificMemberExpression(be.Right, declaringType, memberName))
                throw new Exception("Cannot have 'member' == 'member' in an expression!");

            return IsSpecificMemberExpression(be.Left, declaringType, memberName)
                   || IsSpecificMemberExpression(be.Right, declaringType, memberName);
        }

        internal static bool IsSpecificMemberExpression(Expression exp, Type declaringType, string memberName)
        {
            var memberExpression = exp as MemberExpression;
            return memberExpression != null
                   &&  memberExpression.Member.DeclaringType == declaringType
                   &&  memberExpression.Member.Name == memberName;
        }

        internal static string GetValueFromEqualsExpression(BinaryExpression be, Type memberDeclaringType, string memberName)
        {
            if (be.NodeType != ExpressionType.Equal)
                throw new Exception("There is a bug in this program.");

            if (be.Left.NodeType == ExpressionType.MemberAccess)
            {
                var me = (MemberExpression) be.Left;
                if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
                    return GetValueFromExpression(be.Right);
            }
            else if (be.Right.NodeType == ExpressionType.MemberAccess)
            {
                var me = (MemberExpression) be.Right;
                if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
                    return GetValueFromExpression(be.Left);
            }

            // we should have returned by now.
            throw new Exception("There is a bug in this program.");
        }

        internal static string GetValueFromExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return (string) ((ConstantExpression) expression).Value;

            throw new InvalidQueryException($"The expression type {expression.NodeType} is not supported to obtain a value.");
        }
    }
}

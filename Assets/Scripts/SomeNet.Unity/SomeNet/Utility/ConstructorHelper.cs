using System;
using System.Linq;
using System.Linq.Expressions;

namespace SomeNet.Utility
{
	public static class ConstructorHelper
	{
		public delegate object ConstructorDelegate(params object[] args);

		public static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
		{
			System.Reflection.ConstructorInfo constructor = type.GetConstructor(parameters);
			ParameterExpression paramExpr = Expression.Parameter(typeof(object[]), "args");
			Expression[] argExprs = parameters
				.Select((Type paramType, int index) => (Expression)Expression.Convert(Expression.ArrayAccess(paramExpr, Expression.Constant(index)), paramType))
				.ToArray();
			NewExpression newExpr = Expression.New(constructor, argExprs);
			return Expression.Lambda<ConstructorDelegate>(newExpr, new ParameterExpression[] { paramExpr }).Compile();
		}
	}
}

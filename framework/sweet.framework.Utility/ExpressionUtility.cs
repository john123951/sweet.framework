using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2015/12/14 14:38:01
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Utility
{
    public class ExpressionUtility
    {
        public static Expression<Func<T, TReturn>> FuncToExpression<T, TReturn>(Func<T, TReturn> func)
        {
            return x => func(x);
        }

        public static Func<T, TReturn> FuncToExpression<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return expression.Compile();
        }

        //public static Expression<Func<T, TNewReturn>> TransformReturnType<T, TReturn, TNewReturn>(Expression<Func<T, TReturn>> expression)
        //    where TNewReturn : TReturn
        //{
        //    return x => (TNewReturn)expression.Compile()(x);
        //}

        /// <summary>
        /// Usage:
        ///     Expression<Func<B, bool>> MapFun<A, B>(Expression<Func<A, bool>> input)where A : class
        ///     {
        ///         var result = DbAccessLayer.TransformPredicateLambda<A, B>(input);
        ///         return result;
        ///     }
        /// </summary>
        public static Expression<Func<TNewTarget, bool>> TransformPredicateLambda<TOldTarget, TNewTarget>(Expression<Func<TOldTarget, bool>> predicate)
        {
            var lambda = (LambdaExpression)predicate;
            if (lambda == null)
            {
                throw new NotSupportedException();
            }

            var mutator = new ExpressionTargetTypeMutator(t => typeof(TNewTarget));
            var explorer = new ExpressionTreeExplorer();
            var converted = mutator.Visit(predicate.Body);

            return Expression.Lambda<Func<TNewTarget, bool>>(
                converted,
                lambda.Name,
                lambda.TailCall,
                explorer.Explore(converted).OfType<ParameterExpression>());
        }

        #region 内部类

        private class ExpressionTargetTypeMutator : ExpressionVisitor
        {
            private readonly Func<Type, Type> typeConverter;

            public ExpressionTargetTypeMutator(Func<Type, Type> typeConverter)
            {
                this.typeConverter = typeConverter;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var dataContractType = node.Member.ReflectedType;
                var activeRecordType = this.typeConverter(dataContractType);

                var converted = Expression.MakeMemberAccess(
                    base.Visit(node.Expression),
                    activeRecordType.GetProperty(node.Member.Name));

                return converted;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                var dataContractType = node.Type;
                var activeRecordType = this.typeConverter(dataContractType);

                return Expression.Parameter(activeRecordType, node.Name);
            }
        }

        /// <summary>
        /// Utility class for the traversal of expression trees.
        /// </summary>
        private class ExpressionTreeExplorer
        {
            private readonly Visitor visitor = new Visitor();

            /// <summary>
            /// Returns the enumerable collection of expressions that comprise
            /// the expression tree rooted at the specified node.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>
            /// The enumerable collection of expressions that comprise the expression tree.
            /// </returns>
            public IEnumerable<Expression> Explore(Expression node)
            {
                return this.visitor.Explore(node);
            }

            private class Visitor : ExpressionVisitor
            {
                private readonly List<Expression> expressions = new List<Expression>();

                protected override Expression VisitBinary(BinaryExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitBinary(node);
                }

                protected override Expression VisitBlock(BlockExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitBlock(node);
                }

                protected override Expression VisitConditional(ConditionalExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitConditional(node);
                }

                protected override Expression VisitConstant(ConstantExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitConstant(node);
                }

                protected override Expression VisitDebugInfo(DebugInfoExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitDebugInfo(node);
                }

                protected override Expression VisitDefault(DefaultExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitDefault(node);
                }

                protected override Expression VisitDynamic(DynamicExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitDynamic(node);
                }

                protected override Expression VisitExtension(Expression node)
                {
                    this.expressions.Add(node);
                    return base.VisitExtension(node);
                }

                protected override Expression VisitGoto(GotoExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitGoto(node);
                }

                protected override Expression VisitIndex(IndexExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitIndex(node);
                }

                protected override Expression VisitInvocation(InvocationExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitInvocation(node);
                }

                protected override Expression VisitLabel(LabelExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitLabel(node);
                }

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    this.expressions.Add(node);
                    return base.VisitLambda(node);
                }

                protected override Expression VisitListInit(ListInitExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitListInit(node);
                }

                protected override Expression VisitLoop(LoopExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitLoop(node);
                }

                protected override Expression VisitMember(MemberExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitMember(node);
                }

                protected override Expression VisitMemberInit(MemberInitExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitMemberInit(node);
                }

                protected override Expression VisitMethodCall(MethodCallExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitMethodCall(node);
                }

                protected override Expression VisitNew(NewExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitNew(node);
                }

                protected override Expression VisitNewArray(NewArrayExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitNewArray(node);
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitParameter(node);
                }

                protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitRuntimeVariables(node);
                }

                protected override Expression VisitSwitch(SwitchExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitSwitch(node);
                }

                protected override Expression VisitTry(TryExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitTry(node);
                }

                protected override Expression VisitTypeBinary(TypeBinaryExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitTypeBinary(node);
                }

                protected override Expression VisitUnary(UnaryExpression node)
                {
                    this.expressions.Add(node);
                    return base.VisitUnary(node);
                }

                public IEnumerable<Expression> Explore(Expression node)
                {
                    this.expressions.Clear();
                    this.Visit(node);
                    return expressions.ToArray();
                }
            }
        }

        #endregion 内部类
    }
}
//-----------------------------------------------------------------------
// <copyright file="PropertyAccessor.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyAccessor
{
    /// <summary>
    /// プロパティにアクセスするラムダ式を作成します。
    /// </summary>
    public static class PropertyAccessor
    {
        /// <summary>
        /// プロパティにアクセスするラムダ式を作成します。
        /// </summary>
        /// <typeparam name="TSource">ラムダ式のパラメータとなる型です。</typeparam>
        /// <typeparam name="TResult">アクセス対象のプロパティの型です。</typeparam>
        /// <returns>ラムダの列挙です。対象がない場合、空の列挙を返します。</returns>
        public static IEnumerable<Expression<Func<TSource, TResult>>> GetLambda<TSource, TResult>()
        {
            var arg = Expression.Parameter(typeof(TSource), "x");
            return GetLabda<TSource, TResult>(arg, typeof(TSource).GetProperties(), arg);
        }

        /// <summary>
        /// プロパティにアクセスするラムダ式を作成します。
        /// </summary>
        /// <typeparam name="TSource">ラムダ式のパラメータとなる型です。</typeparam>
        /// <typeparam name="TResult">アクセス対象のプロパティの型です。</typeparam>
        /// <param name="arg">パラメータ用の<see cref="ParameterExpression"/>です。</param>
        /// <param name="propertyInfo">プロパティです。</param>
        /// <param name="member">プロパティへの<see cref="Expression"/>です。</param>
        /// <returns>ラムダの列挙です。対象がない場合、空の列挙を返します。</returns>
        private static IEnumerable<Expression<Func<TSource, TResult>>> GetLabda<TSource, TResult>(
            ParameterExpression arg,
            PropertyInfo[] propertyInfo,
            Expression member)
        {
            var result = new List<Expression<Func<TSource, TResult>>>();

            propertyInfo.ToList()
                .ForEach(prop =>
                {
                    var newMember = Expression.PropertyOrField(member, prop.Name);
                    if (prop.PropertyType == typeof(TResult))
                    {
                        result.Add(Expression.Lambda<Func<TSource, TResult>>(newMember, arg));
                        return;
                    }

                    result.AddRange(GetLabda<TSource, TResult>(arg, prop.PropertyType.GetProperties(), newMember));
                });
            return result;
        }
    }
}

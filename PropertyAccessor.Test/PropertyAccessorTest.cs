//-----------------------------------------------------------------------
// <copyright file="PropertyAccessorTest.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace PropertyAccessor
{
    /// <summary>
    /// <see cref="PropertyAccessor"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class PropertyAccessorTest
    {
        /// <summary>
        /// <see cref="PropertyAccessor.GetLambda{TSource, TResult}"/>のテストメソッドです。
        /// </summary>
        /// <param name="index">列挙のインデックスです。</param>
        /// <param name="expected">期待値です。</param>
        [DataTestMethod]
        [DataRow(0, "A")]
        [DataRow(1, "B-C")]
        [DataRow(2, "B-D")]
        public void Test(int index, string expected)
        {
            var hoge = new Hoge
            {
                A = "A",
                B = new Foo
                {
                    C = "B-C",
                    D = "B-D",
                }
            };

            var actual = PropertyAccessor.GetLambda<Hoge, string>().ElementAt(index);
            var func = actual.Compile();
            Assert.AreEqual(expected, func(hoge), actual.ToString());
        }

        /// <summary>
        /// ダミークラスです。
        /// </summary>
        private class Hoge
        {
            /// <summary>
            /// 検出対象のプロパティです。
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// このプロパティの中身も検出対象です。
            /// </summary>
            public Foo B { get; set; }
        }

        /// <summary>
        /// ダミークラスです。
        /// </summary>
        private class Foo
        {
            /// <summary>
            /// 検出対象のプロパティです。
            /// </summary>
            public string C { get; set; }

            /// <summary>
            /// 検出対象のプロパティです。
            /// </summary>
            public string D { get; set; }

            /// <summary>
            /// 検出対象外のプロパティです。
            /// </summary>
            public bool E { get; set; }
        }
    }
}

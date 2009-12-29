// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace NUnit.Core
{
	/// <summary>
	/// TestFixture is a surrogate for a user test fixture class,
	/// containing one or more tests.
	/// </summary>
	public class TestFixture : TestSuite
	{
		#region Constructors
        public TestFixture(Type fixtureType)
            : this(fixtureType, null) { }

        public TestFixture(Type fixtureType, object[] arguments)
            : base(fixtureType, arguments) 
        {
            this.fixtureSetUpMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, typeof(TestFixtureSetUpAttribute), true);
            this.fixtureTearDownMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, typeof(TestFixtureTearDownAttribute), true);
            this.setUpMethods =
                Reflect.GetMethodsWithAttribute(this.FixtureType, typeof(SetUpAttribute), true);
            this.tearDownMethods =
                Reflect.GetMethodsWithAttribute(this.FixtureType, typeof(TearDownAttribute), true);
        }
        #endregion

		#region TestSuite Overrides
        public override TestResult Run(ITestListener listener, TestFilter filter)
        {
            using ( new DirectorySwapper( AssemblyHelper.GetDirectoryName( FixtureType.Assembly ) ) )
            {
                return base.Run(listener, filter);
            }
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult)
        {
            base.DoOneTimeSetUp(suiteResult);

            suiteResult.AssertCount = Assert.Counter; ;
        }

        protected override void DoOneTimeTearDown(TestResult suiteResult)
        {
            base.DoOneTimeTearDown(suiteResult);

            suiteResult.AssertCount += Assert.Counter;
        }
        #endregion
	}
}
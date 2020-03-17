using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Entity.Tests.Common
{
    public class TestData<T>
    {
        public T Value { get; }
        public TestData(T defaultValue)
        {
            Value = defaultValue;
        }

        public TestData<T> SetProperty<TValue>(Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(Value, value, null);
                }
            }
            return this;
        }
    }
}

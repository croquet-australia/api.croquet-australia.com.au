using System;
using CroquetAustralia.Library;
using OpenMagic;

namespace CroquetAustralia.TestHelpers
{
    public class CommonTestBase
    {
        protected Exception CaughtException;

        protected CommonTestBase(IServiceProvider serviceProvider)
        {
            ServiceFactory.SetServiceProvider(serviceProvider);
        }

        protected IDummy Dummy => ServiceFactory.Get<IDummy>();

        protected T Get<T>()
        {
            return ServiceFactory.Get<T>();
        }

        protected T Invalid<T>(string propertyName, object propertyValue) where T : class
        {
            return Valid<T>().SetProperty(propertyName, propertyValue);
        }

        protected void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (AggregateException exception)
            {
                throw new Exception("Aggregate Exception", exception.InnerException);
            }
        }

        protected virtual T Valid<T>() where T : class
        {
            var value = (T)Dummy.Value(typeof(T));
            return value;
        }
    }
}
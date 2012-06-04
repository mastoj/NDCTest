using System;
using NUnit.Framework;

namespace NDCTest
{
    public interface IAmSpecification
    {
        void Setup();
        object GetGiven();
        Command When();
    }

    [TestFixture]
    public abstract class Specifiation<T> : IAmSpecification
    {
        public Exception ThrownException { get; private set; }
        public T SUT { get; set; }
        protected abstract T Given();
        public abstract Command When(T context);

        [TestFixtureSetUp]
        public void Setup()
        {
            SUT = Given();
            Command = When(SUT);
            try
            {
                Command.Execute();
            }
            catch (Exception exception)
            {
                ThrownException = exception;
            }
        }

        public Command Command { get; private set; }
        public object GetGiven()
        {
            return SUT;
        }

        public Command When()
        {
            return Command;
        }
    }
}
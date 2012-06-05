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
    public abstract class Specification<T, TResult> : IAmSpecification
    {
        public Exception ThrownException { get; private set; }
        public T SUT { get; set; }
        protected abstract T Given();
        public abstract Command<TResult> When(T context);
        public TResult Result { get; private set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            SUT = Given();
            Command = When(SUT);
            try
            {
                Result = Command.Execute();
            }
            catch (Exception exception)
            {
                ThrownException = exception;
            }
        }

        public Command<TResult> Command { get; private set; }
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
namespace NDCTest
{
    public abstract class Command<TResult> : Command
    {
        public abstract TResult Execute();
    }

    public abstract class Command
    {
    }
}
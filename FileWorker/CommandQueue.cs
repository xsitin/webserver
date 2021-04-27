using System.Collections.Generic;

namespace oop
{
    public class CommandQueue
    {
        private static CommandQueue? instance;
        private Queue<ICommand> _commands = new Queue<ICommand>();

        private CommandQueue(){}

        public static CommandQueue GetInstance()
        {
            instance ??= new CommandQueue();
            return instance;
        }

        public void Push(ICommand command)
        {
            _commands.Enqueue(command);
        }

        public void Run()
        {
            while (_commands.Count != 0)
            {
                _commands.Dequeue().Execute();
            }
        }
    }
}
using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder fileLogger = new Pathfinder(new FileLogWriter());
            Pathfinder consoleLogger = new Pathfinder(new ConsoleLogWriter());
            Pathfinder fileLoggerOnFridays = new Pathfinder(new SecureLogWriter(new FileLogWriter()));
            Pathfinder consoleLoggerOnFridays = new Pathfinder(new SecureLogWriter(new ConsoleLogWriter()));
            Pathfinder consoleAndFileLoggerOnFridays = new Pathfinder(new CombinedLogWriter(new ConsoleLogWriter(), new FileLogWriter()));

            fileLogger.Find("В файл");
            consoleLogger.Find("В консоль");
            fileLoggerOnFridays.Find("В файл по пятницам");
            consoleLoggerOnFridays.Find("В консоль по пятницам");
            consoleAndFileLoggerOnFridays.Find("В консоль и файл по пятницам");
        }
    }

    public interface ILogger
    {
        void WriteError(string message);
    }

    class ConsoleLogWriter : ILogger
    {
        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWriter : ILogger
    {
        public virtual void WriteError(string message)
        {
            File.AppendAllText("log.txt", message + Environment.NewLine);
        }
    }

    class SecureLogWriter : ILogger
    {
        private readonly ILogger _innerLogger;

        public SecureLogWriter(ILogger innerLogger)
        {
            _innerLogger = innerLogger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _innerLogger.WriteError(message);
            }
        }
    }

    class CombinedLogWriter : ILogger
    {
        private readonly ILogger _consoleLogger;
        private readonly ILogger _fileLogger;

        public CombinedLogWriter(ILogger consoleLogger, ILogger fileLogger)
        {
            _consoleLogger = consoleLogger;
            _fileLogger = fileLogger;
        }

        public void WriteError(string message)
        {
            _consoleLogger.WriteError(message);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _fileLogger.WriteError(message);
            }
        }
    }

    class Pathfinder
    {
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string message)
        {
            _logger.WriteError(message);
        }
    }
}
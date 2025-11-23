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
            Pathfinder fileLoggerOnFridays = new Pathfinder(new SecureFileLogWriter());
            Pathfinder consoleLoggerOnFridays = new Pathfinder(new SecureConsoleLogWriter());
            Pathfinder consoleAndFileLoggerOnFridays = new Pathfinder(new CombinedSecureLogWriter());

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

    class SecureConsoleLogWriter : ConsoleLogWriter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class SecureFileLogWriter : FileLogWriter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class CombinedSecureLogWriter : ILogger
    {
        private readonly ConsoleLogWriter _consoleLogWriter;
        private readonly FileLogWriter _fileLogWriter;

        public CombinedSecureLogWriter()
        {
            _consoleLogWriter = new ConsoleLogWriter();
            _fileLogWriter = new FileLogWriter();
        }

        public void WriteError(string message)
        {
            _consoleLogWriter.WriteError(message);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _fileLogWriter.WriteError(message);
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
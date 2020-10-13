using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SPP2
{

    class tasks
    {
        public tasks()
        {
            for (int i = 0; i < 50; i++)
                _ = helloworld();
        }

        private async Task helloworld()
        {
            await hi();
        }

        private async Task hi()
        {
            await Task.Delay(2000); // wait some logic
            Console.WriteLine("hello world");
        }

    }
    class TaskQueue
    {
        //- называется TaskQueue и реализует логику пула потоков;
        delegate void TaskDelegate();

        public TaskQueue(int threadsCount)
        {
            Queue<TaskDelegate> taskQueue = new Queue<TaskDelegate>();
            for (int i = 0; i < threadsCount; i++)
            {
                taskQueue.Enqueue(HelloWorld);
            }
            ThreadPool.SetMaxThreads(threadsCount, threadsCount);
            while (taskQueue.Count > 0)
            {
                EnqueueTask(taskQueue.Dequeue());
            }
            //-создает указанное количество потоков пула в конструкторе;
            //-содержит очередь задач в виде делегатов без параметров:
        }

        //- обеспечивает постановку в очередь и последующее выполнение делегатов с помощью метода
        void EnqueueTask(TaskDelegate task)
        {
            ThreadPool.QueueUserWorkItem(_ => { HelloWorld(); });
        }

        void HelloWorld()
        {
            Console.WriteLine($"Hello world in {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
        }


    }
    class Second
    {
        static CountdownEvent CountDown;
        public Second(string fromPath, string toPath)
        {
            var fromFiles = Directory.GetFiles(fromPath);
            CountDown = new CountdownEvent(fromFiles.Count());
            foreach (var item in fromFiles)
            {
                ThreadPool.QueueUserWorkItem(_ => MoveFile(item, toPath));
            }
            CountDown.Wait();
            Console.WriteLine("Done");

        }
        public void MoveFile(string from, string to)
        {
            var fileName = "\\" + Path.GetFileName(from);
            to += fileName;
            File.Move(from, to);
            Console.WriteLine($"Move {from} to {to} in thread #{Thread.CurrentThread.ManagedThreadId}");
            CountDown.Signal();
        }

    }

    //    Реализовать консольную программу на языке C#, которая:
    //- принимает в параметре командной строки путь к исходному и целевому каталогам на диске;
    //- выполняет параллельное копирование всех файлов из исходного каталога в целевой каталог;
    //- выполняет операции копирования параллельно с помощью пула потоков;
    //- дожидается окончания всех операций копирования и выводит в консоль информацию о количестве скопированных файлов.
    class Program
    {
        static void Main(string[] args)
        {
            new tasks();
                        //  new TaskQueue(50);//First
                              //   new Second(args[0], args[1]);
                              // new Second("D:/SPP2/From","D:/SPP2/To");
            Console.ReadKey();
        }

    }

}

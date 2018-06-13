using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace TimerDemo
{
    class Program
    {
        static Timer timeoutTimer;
        static void Main(string[] args)
        {
            //普通调用
            var timer = new Timer(CommonWriteSomeThing, Guid.NewGuid(),0,2000);

            //防止回调方法被调用多次
            timeoutTimer = new Timer(TimeoutWriteSomeThing, Guid.NewGuid(), 0, Timeout.Infinite);//周期为无限期

            //推荐使用任务
            SchedulerTask();

            Console.ReadLine();
            timer.Dispose();//此处添加对timer的引用，避免timer被提前回收
        }

        private static void CommonWriteSomeThing(object state)
        {
            Console.WriteLine($"普通定时：{DateTime.Now.ToString()};{state.ToString()}");
            GC.Collect();//模拟一次强制垃圾回收
        }

        private static void TimeoutWriteSomeThing(object state)
        {
            Console.WriteLine($"回调过长：{DateTime.Now.ToString()};{state.ToString()}");
            Thread.Sleep(3000);//模拟计算限制的操作
            if(timeoutTimer!=null)
            {
                timeoutTimer.Change(0, Timeout.Infinite);//周期为无限期
            }

        }

        async static void SchedulerTask()
        {
            while (true)
            {
                TaskWriteSomeThing(Guid.NewGuid());
                await Task.Delay(500);
            }           
        }
        private static void TaskWriteSomeThing(object state)
        {
            Console.WriteLine($"Task定时：{DateTime.Now.ToString()};{state.ToString()}");
            Thread.Sleep(3000);
        }
    }
}

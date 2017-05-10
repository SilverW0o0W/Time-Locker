using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using LoggerManager;

namespace TimerManager
{
    public class TimerController
    {
        public TimeSpan SleepTime { get; private set; }
        private static Logger log = new Logger();
        private Dictionary<string, DispatcherTimer> timers = new Dictionary<string, DispatcherTimer>();

        public Guid Start()
        {
            return Start(SleepTime, Timer_Tick);
        }

        public Guid Start(TimeSpan timeSpan)
        {
            return Start(timeSpan, Timer_Tick);
        }

        private Guid Start(TimeSpan timeSpan, Action<object, EventArgs> tick)
        {
            DispatcherTimer timer = new DispatcherTimer();
            Guid guid = Guid.NewGuid();
            timer.Tick += new EventHandler(tick);
            timers.Add(guid.ToString(), timer);
            timer.Interval = timeSpan;
            timer.Start();
            log.Info("Start a new timer. GUID: {0}.TimeSpan: {1}", guid, timeSpan.ToString());
            return guid;
        }

        public void Stop(Guid guid)
        {
            if (timers.Count < 1)
            {
                return;
            }
            DispatcherTimer timer = null;
            string id = guid.ToString();
            try
            {
                if (timers.ContainsKey(id))
                {
                    timer = timers[id];
                    timer.Stop();
                    timers.Remove(id);
                }
            }
            catch (Exception ex)
            {
                log.Error("A exception has occured when timer stop. GUID :{0}. Reason: {1}.", guid, ex.ToString());
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

        }
    }
}

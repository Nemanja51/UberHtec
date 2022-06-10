using System;
using System.Timers;

namespace Uber.Domain.Helpers
{
    public static class TimerHelper
    {
        private static Timer aTimer;
        private static int resId;

        public static void SetTimer(int reservationId)
        {
            resId = reservationId;

            // Create a timer with a two minutes interval.
            aTimer = new Timer(120000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public static void StopTimer() 
        {
            aTimer.Stop();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //decline reservation authomaticaly
        }
    }
}

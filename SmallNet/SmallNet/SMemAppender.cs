using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Appender;

namespace SmallNet
{
    /// <summary>
    /// grab log4net output and huck it into memory. HUCK IT.
    /// </summary>
    public class SMemAppender : MemoryAppender
    {
        public event EventHandler Updated;

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            // Append the event as usual
            base.Append(loggingEvent);

            // Then alert the Updated event that an event has occurred
            var handler = Updated;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

    }
}

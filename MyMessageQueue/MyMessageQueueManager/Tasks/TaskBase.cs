using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.MessageQueue.Tasks
{
    public class TaskBase
    {

        protected MyManagerCSharp.Log.LogManager _log;
        protected string _taskName;
        protected Guid _uid;


    }
}

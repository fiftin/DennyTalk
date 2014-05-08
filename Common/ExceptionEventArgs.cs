using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception error)
        {
            this.error = error;
        }

        private Exception error;

        public Exception Error
        {
            get { return error; }
        }

    }
}

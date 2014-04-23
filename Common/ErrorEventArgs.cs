using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception error)
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

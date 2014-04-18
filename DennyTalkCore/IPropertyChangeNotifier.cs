using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class PropertyChangeNotifierEventArgs : EventArgs
    {
        public PropertyChangeNotifierEventArgs(string propertyName, object oldValue, object newValue)
        {
            this.propertyName = propertyName;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        private string propertyName;
        private object oldValue;
        private object newValue;

        public object NewValue
        {
            get { return newValue; }
        }

        public object OldValue
        {
            get { return oldValue; }
        }

        public string PropertyName
        {
            get { return propertyName; }
        }

    }

    public interface IPropertyChangeNotifier
    {
        event EventHandler<PropertyChangeNotifierEventArgs> PropertyChange;
    }
}

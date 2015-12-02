using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace labs.Graph
{
    [Serializable]
    public class property_base : System.ComponentModel.INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null && !String.IsNullOrWhiteSpace(propertyName))
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

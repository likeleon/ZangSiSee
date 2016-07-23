using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZangSiSee
{
    public interface IDirty
    {
        bool IsDirty { get; set; }
    }

    public class BaseNotify : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseNotify() {}

        public virtual void Dispose()
        {
            ClearEvents();
        }

        void ClearEvents()
        {
            foreach (var p in PropertyChanged?.GetInvocationList())
                PropertyChanged -= (PropertyChangedEventHandler)p;
        }

        internal bool SetPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;

            var dirty = this as IDirty;
            if (dirty != null)
                dirty.IsDirty = true;

            SetPropertyChanged(propertyName);
            return true;
        }

        internal void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

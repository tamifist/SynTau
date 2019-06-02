using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Connectivity;

namespace AutoGene.Mobile.Abstractions
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _propTitle = string.Empty;
        string _userId = string.Empty;
        bool _propIsBusy;

        public string Title
        {
            get { return _propTitle; }
            set { SetProperty(ref _propTitle, value, nameof(Title)); }
        }

        public bool IsBusy
        {
            get { return _propIsBusy; }
            set { SetProperty(ref _propIsBusy, value, nameof(IsBusy)); }
        }

        public bool IsOffline
        {
            get
            {
                return !CrossConnectivity.Current.IsConnected;
            }
        }

        public async Task<bool> IsLocalUrlReachable(string url)
        {
            return await CrossConnectivity.Current.IsReachable(url);
        }

        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value, nameof(UserId)); }
        }

        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
            {
                return;
            }
            store = value;
            if (onChanged != null)
            {
                onChanged();
            }
            OnPropertyChanged(propName);
        }

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
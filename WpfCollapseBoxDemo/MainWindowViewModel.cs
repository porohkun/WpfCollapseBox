using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfCollapseBoxDemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isChecked = true;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _collapsedHeight = 20;

        public double CollapsedHeight
        {
            get => _collapsedHeight;
            set
            {
                if (_collapsedHeight != value)
                {
                    _collapsedHeight = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _expandedHeight = 200;

        public double ExpandedHeight
        {
            get => _expandedHeight;
            set
            {
                if (_expandedHeight != value)
                {
                    _expandedHeight = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}

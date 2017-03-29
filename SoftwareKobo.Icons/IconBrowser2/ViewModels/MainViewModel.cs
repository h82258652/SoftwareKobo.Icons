using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IconBrowser2.Models;
using IconBrowser2.Services;
using SoftwareKobo.Icons.Dashicons;
using SoftwareKobo.Icons.FontAwesome;
using SoftwareKobo.Icons.Ionicons;
using SoftwareKobo.Icons.MaterialIcons;

namespace IconBrowser2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IReadOnlyList<Icon> _allIcons;

        private readonly IClipboardService _clipboardService;

        private RelayCommand _copyCommand;

        private RelayCommand<Icon> _iconClickCommand;

        private string _query;

        private Icon _selectedIcon;

        public MainViewModel(IClipboardService clipboardService)
        {
            _clipboardService = clipboardService;

            var allIcons = new List<Icon>();
            allIcons.AddRange(Enum.GetNames(typeof(Symbol)).Select(temp => new Icon()
            {
                EnumType = typeof(Symbol),
                Name = temp
            }));
            allIcons.AddRange(Enum.GetNames(typeof(FontAwesomeSymbol)).Select(temp => new Icon()
            {
                EnumType = typeof(FontAwesomeSymbol),
                Name = temp
            }));
            allIcons.AddRange(Enum.GetNames(typeof(MaterialSymbol)).Select(temp => new Icon()
            {
                EnumType = typeof(MaterialSymbol),
                Name = temp
            }));
            allIcons.AddRange(Enum.GetNames(typeof(DashSymbol)).Select(temp => new Icon()
            {
                EnumType = typeof(DashSymbol),
                Name = temp
            }));
            allIcons.AddRange(Enum.GetNames(typeof(IonicSymbol)).Select(temp => new Icon()
            {
                EnumType = typeof(IonicSymbol),
                Name = temp
            }));
            _allIcons = allIcons;
        }

        public RelayCommand CopyCommand
        {
            get
            {
                _copyCommand = _copyCommand ?? new RelayCommand(() =>
                {
                    var icon = SelectedIcon;
                    if (icon == null)
                    {
                        return;
                    }

                    var text = new StringBuilder();
                    var html = new StringBuilder();
                    html.Append("<div>");
                    text.Append("<");
                    html.Append("<span style=\"color:blue;\">&lt;</span>");
                    string className;
                    if (icon.EnumType == typeof(Symbol))
                    {
                        className = nameof(SymbolIcon);
                    }
                    else if (icon.EnumType == typeof(FontAwesomeSymbol))
                    {
                        className = nameof(FontAwesomeIcon);
                    }
                    else if (icon.EnumType == typeof(MaterialSymbol))
                    {
                        className = nameof(MaterialIcon);
                    }
                    else if (icon.EnumType == typeof(DashSymbol))
                    {
                        className = nameof(DashIcon);
                    }
                    else if (icon.EnumType == typeof(IonicSymbol))
                    {
                        className = nameof(IonicSymbol);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(icon.EnumType));
                    }
                    text.Append(className);
                    html.Append("<span style=\"color:#a31515;\">" + className + "</span>");
                    text.Append(" Symbol");
                    html.Append("<span style=\"color:red;\">&nbsp;Symbol</span>");
                    text.Append("=\"" + icon.Name + "\" />");
                    html.Append("<span style=\"color:blue;\">=&quot;" + icon.Name + "&quot; /&gt;</span>");
                    html.Append("</div>");
                    _clipboardService.SetHtml(text.ToString(), html.ToString());
                }, () => SelectedIcon != null);
                return _copyCommand;
            }
        }

        public RelayCommand<Icon> IconClickCommand
        {
            get
            {
                _iconClickCommand = _iconClickCommand ?? new RelayCommand<Icon>(icon =>
                {
                    SelectedIcon = icon;
                });
                return _iconClickCommand;
            }
        }

        public IEnumerable<Icon> Icons
        {
            get
            {
                if (string.IsNullOrEmpty(Query))
                {
                    return _allIcons;
                }
                else
                {
                    return _allIcons.Where(temp => temp.Name.IndexOf(Query, StringComparison.OrdinalIgnoreCase) > -1);
                }
            }
        }

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                if (_query != value)
                {
                    Set(ref _query, value);
                    RaisePropertyChanged(nameof(Icons));
                }
            }
        }

        public Icon SelectedIcon
        {
            get
            {
                return _selectedIcon;
            }
            private set
            {
                Set(ref _selectedIcon, value);
                CopyCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using SoftwareKobo.Icons.Dashicons;
using SoftwareKobo.Icons.FontAwesome;
using SoftwareKobo.Icons.MaterialIcons;
using WinRTXamlToolkit.Imaging;

namespace IconBrowser
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            UpdateIcons();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateIcons();
        }

        private IEnumerable<Icon> GetIcons<T>(string filter)
        {
            var names = Enum.GetNames(typeof(T));
            if (string.IsNullOrEmpty(filter))
            {
                return names.Select(temp => new Icon()
                {
                    EnumType = typeof(T),
                    Name = temp
                });
            }
            else
            {
                return names.Where(temp => temp.IndexOf(filter, StringComparison.OrdinalIgnoreCase) > -1).Select(temp => new Icon()
                {
                    EnumType = typeof(T),
                    Name = temp
                });
            }
        }

        private void UpdateIcons()
        {
            var filter = AutoSuggestBox.Text;
            var icons = new List<Icon>();
            icons.AddRange(GetIcons<Symbol>(filter));
            icons.AddRange(GetIcons<FontAwesomeSymbol>(filter));
            icons.AddRange(GetIcons<MaterialSymbol>(filter));
            icons.AddRange(GetIcons<DashSymbol>(filter));
            GridView.ItemsSource = icons;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var icon = e.ClickedItem as Icon;
            if (icon != null)
            {
                if (icon.EnumType == typeof(Symbol))
                {
                    SymbolTextBlock.Text = string.Format("<SymbolIcon Symbol=\"{0}\" />", icon.Name);
                }
                else if (icon.EnumType == typeof(FontAwesomeSymbol))
                {
                    SymbolTextBlock.Text = string.Format("<FontAwesomeIcon Symbol=\"{0}\" />", icon.Name);
                }
                else if (icon.EnumType == typeof(MaterialSymbol))
                {
                    SymbolTextBlock.Text = string.Format("<MaterialIcon Symbol=\"{0}\" />", icon.Name);
                }
                else if (icon.EnumType == typeof(DashSymbol))
                {
                    SymbolTextBlock.Inlines.Clear();
                    SymbolTextBlock.Inlines.Add(new Run()
                    {
                        Text = "<",
                        Foreground = new SolidColorBrush(ColorExtensions.FromString("#0000ff"))
                    });
                    SymbolTextBlock.Inlines.Add(new Run()
                    {
                        Text = "DashIcon",
                        Foreground = new SolidColorBrush(ColorExtensions.FromString("#a31515"))
                    });
                    SymbolTextBlock.Inlines.Add(new Run()
                    {
                        Text = " Symbol",
                        Foreground = new SolidColorBrush(ColorExtensions.FromString("#ff0000"))
                    });
                    SymbolTextBlock.Inlines.Add(new Run()
                    {
                        Text = "=\"" + icon.Name + "\" />",
                        Foreground = new SolidColorBrush(ColorExtensions.FromString("#0000ff"))
                    });
                }
                else
                {
                    SymbolTextBlock.Text = string.Empty;
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(SymbolTextBlock.Text);
            Clipboard.SetContent(dataPackage);
        }
    }
}
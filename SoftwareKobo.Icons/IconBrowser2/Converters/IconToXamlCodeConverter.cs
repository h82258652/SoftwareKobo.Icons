using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using IconBrowser2.Models;
using SoftwareKobo.Icons.Dashicons;
using SoftwareKobo.Icons.FontAwesome;
using SoftwareKobo.Icons.Ionicons;
using SoftwareKobo.Icons.MaterialIcons;

namespace IconBrowser2.Converters
{
    public class IconToXamlCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = value as Icon;
            if (icon == null)
            {
                return null;
            }

            var xamlCodeTextBlock = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap
            };
            xamlCodeTextBlock.Inlines.Add(new Run()
            {
                Text = "<",
                Foreground = new SolidColorBrush(Colors.Blue)
            });
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
                className = nameof(IonicIcon);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(icon.EnumType));
            }
            xamlCodeTextBlock.Inlines.Add(new Run()
            {
                Text = className,
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA3, 0x15, 0x15))
            });
            xamlCodeTextBlock.Inlines.Add(new Run()
            {
                Text = " Symbol",
                Foreground = new SolidColorBrush(Colors.Red)
            });
            xamlCodeTextBlock.Inlines.Add(new Run()
            {
                Text = "=\"" + icon.Name + "\" />",
                Foreground = new SolidColorBrush(Colors.Blue)
            });
            return xamlCodeTextBlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using IconBrowser2.Models;
using SoftwareKobo.Icons.Dashicons;
using SoftwareKobo.Icons.FontAwesome;
using SoftwareKobo.Icons.Ionicons;
using SoftwareKobo.Icons.MaterialIcons;

namespace IconBrowser2.Converters
{
    public class IconToElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = value as Icon;
            if (icon == null)
            {
                return null;
            }

            var symbol = Enum.Parse(icon.EnumType, icon.Name);
            if (icon.EnumType == typeof(Symbol))
            {
                return new SymbolIcon((Symbol)symbol);
            }
            else if (icon.EnumType == typeof(FontAwesomeSymbol))
            {
                return new FontAwesomeIcon((FontAwesomeSymbol)symbol);
            }
            else if (icon.EnumType == typeof(MaterialSymbol))
            {
                return new MaterialIcon((MaterialSymbol)symbol);
            }
            else if (icon.EnumType == typeof(DashSymbol))
            {
                return new DashIcon((DashSymbol)symbol);
            }
            else if (icon.EnumType == typeof(IonicSymbol))
            {
                return new IonicIcon((IonicSymbol)symbol);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
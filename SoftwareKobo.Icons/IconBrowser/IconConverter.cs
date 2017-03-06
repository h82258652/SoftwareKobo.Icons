using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using SoftwareKobo.Icons.Dashicons;
using SoftwareKobo.Icons.FontAwesome;
using SoftwareKobo.Icons.MaterialIcons;

namespace IconBrowser
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = (Icon)value;
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
            else
            {
                return icon.Name;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
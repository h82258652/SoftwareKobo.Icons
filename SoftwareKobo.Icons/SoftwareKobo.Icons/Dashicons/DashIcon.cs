using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.Icons.Dashicons
{
    public sealed class DashIcon : FontIcon
    {
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol), typeof(DashSymbol), typeof(DashIcon), new PropertyMetadata(default(DashSymbol), SymbolChanged));

        private static readonly FontFamily DashiconsFontFamily = new FontFamily(Constants.DashiconsFamilyName);

        public DashIcon()
        {
            FontFamily = DashiconsFontFamily;
        }

        public DashIcon(DashSymbol symbol) : this()
        {
            Symbol = symbol;
        }

        public new FontFamily FontFamily
        {
            get
            {
                if (base.FontFamily != DashiconsFontFamily)
                {
                    base.FontFamily = DashiconsFontFamily;
                }
                return DashiconsFontFamily;
            }
            private set
            {
                base.FontFamily = value;
            }
        }

        public DashSymbol Symbol
        {
            get
            {
                return (DashSymbol)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        private static void SymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DashIcon)d;
            var value = (DashSymbol)e.NewValue;

            obj.Glyph = char.ConvertFromUtf32((int)value);
        }
    }
}
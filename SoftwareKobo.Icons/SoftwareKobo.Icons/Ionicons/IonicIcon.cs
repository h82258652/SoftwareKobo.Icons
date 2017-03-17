using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.Icons.Ionicons
{
    public sealed class IonicIcon : FontIcon
    {
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol), typeof(IonicSymbol), typeof(IonicIcon), new PropertyMetadata(default(IonicSymbol), SymbolChanged));

        private static readonly FontFamily IoniconsFontFamily = new FontFamily(Constants.IoniconsFamilyName);

        public IonicIcon()
        {
            FontFamily = IoniconsFontFamily;
        }

        public IonicIcon(IonicSymbol symbol) : this()
        {
            Symbol = symbol;
        }

        public new FontFamily FontFamily
        {
            get
            {
                if (base.FontFamily != IoniconsFontFamily)
                {
                    base.FontFamily = IoniconsFontFamily;
                }
                return IoniconsFontFamily;
            }
            private set
            {
                base.FontFamily = value;
            }
        }

        public IonicSymbol Symbol
        {
            get
            {
                return (IonicSymbol)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        private static void SymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (IonicIcon)d;
            var value = (IonicSymbol)e.NewValue;

            obj.Glyph = char.ConvertFromUtf32((int)value);
        }
    }
}
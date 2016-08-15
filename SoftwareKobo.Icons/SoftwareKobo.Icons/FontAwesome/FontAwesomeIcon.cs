using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.Icons.FontAwesome
{
    public sealed class FontAwesomeIcon : FontIcon
    {
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol), typeof(FontAwesomeSymbol), typeof(FontAwesomeIcon), new PropertyMetadata(default(FontAwesomeSymbol), SymbolChanged));

        private static readonly FontFamily FontAwesomeFontFamily = new FontFamily(Constants.FontAwesomeFamilyName);

        public FontAwesomeIcon()
        {
            FontFamily = FontAwesomeFontFamily;
        }

        public FontAwesomeIcon(FontAwesomeSymbol symbol) : this()
        {
            Symbol = symbol;
        }

        public new FontFamily FontFamily
        {
            get
            {
                if (base.FontFamily != FontAwesomeFontFamily)
                {
                    base.FontFamily = FontAwesomeFontFamily;
                }
                return FontAwesomeFontFamily;
            }
            private set
            {
                base.FontFamily = value;
            }
        }

        public FontAwesomeSymbol Symbol
        {
            get
            {
                return (FontAwesomeSymbol)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        private static void SymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (FontAwesomeIcon)d;
            var value = (FontAwesomeSymbol)e.NewValue;

            obj.Glyph = char.ConvertFromUtf32((int)value);
        }
    }
}
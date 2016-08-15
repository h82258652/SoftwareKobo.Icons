using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.Icons.MaterialIcons
{
    public sealed class MaterialIcon : FontIcon
    {
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol), typeof(MaterialSymbol), typeof(MaterialIcon), new PropertyMetadata(default(MaterialSymbol), SymbolChanged));

        private static readonly FontFamily MaterialIconFontFamily = new FontFamily(Constants.MaterialIconsFamilyName);

        public MaterialIcon()
        {
            FontFamily = MaterialIconFontFamily;
        }

        public MaterialIcon(MaterialSymbol symbol) : this()
        {
            Symbol = symbol;
        }

        public new FontFamily FontFamily
        {
            get
            {
                if (base.FontFamily != MaterialIconFontFamily)
                {
                    base.FontFamily = MaterialIconFontFamily;
                }
                return MaterialIconFontFamily;
            }
            private set
            {
                base.FontFamily = value;
            }
        }

        public MaterialSymbol Symbol
        {
            get
            {
                return (MaterialSymbol)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        private static void SymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (MaterialIcon)d;
            var value = (MaterialSymbol)e.NewValue;

            obj.Glyph = char.ConvertFromUtf32((int)value);
        }
    }
}
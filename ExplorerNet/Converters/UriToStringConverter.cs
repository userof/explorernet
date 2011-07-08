using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ExplorerNet.Converters
{
    public class UriToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts <paramref name="value"/> to string.
        /// </summary>
        /// <param name="value"><see cref="Uri"/>.</param>
        /// <param name="targetType">Ignored.</param>
        /// <param name="parameter">Ignored.</param>
        /// <param name="culture">Ignored.</param>
        /// <returns>
        /// If <paramref name="value"/> is not <see cref="Uri"/>, returns null.
        /// If <see cref="Uri.IsFile"/> is true, returns <see cref="Uri.LocalPath"/>, otherwise unescaped (<see cref="Uri.UnescapeDataString"/>) <see cref="Uri.AbsoluteUri"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value as string;
            if (string.IsNullOrEmpty(text))
                return null;
            return new Uri(text);

            
        }
        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">Text.</param>
        /// <param name="targetType">Ignored.</param>
        /// <param name="parameter">Ignored.</param>
        /// <param name="culture">Ignored.</param>
        /// <returns>
        /// If <paramref name="value"/> is not string or is empty, returns null, otherwise new <see cref="Uri"/> from the string.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = value as Uri;
            if (uri == null)
                return null;
            string text;
            if (uri.IsFile)
                text = uri.LocalPath;
            else
                text = Uri.UnescapeDataString(uri.AbsoluteUri);
            return text;
        }
    }
}

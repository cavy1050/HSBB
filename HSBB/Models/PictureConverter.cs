using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HSBB.Models
{
    public class PictureConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage musicPicture = new BitmapImage();

            if (value != null)
            {
                byte[] image = System.Convert.FromBase64String((string)value);

                if (image != null)
                {
                    MemoryStream ms = new MemoryStream(image);
                    musicPicture = new BitmapImage();
                    musicPicture.BeginInit();
                    musicPicture.StreamSource = ms;
                    musicPicture.EndInit();
                }
            }

            return musicPicture;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}

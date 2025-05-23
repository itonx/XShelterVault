using Microsoft.UI.Xaml.Data;

namespace XShelterVault.Converters
{
    public class BoolToPasswordVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool toggleValue && toggleValue) return PasswordRevealMode.Visible;

            return PasswordRevealMode.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

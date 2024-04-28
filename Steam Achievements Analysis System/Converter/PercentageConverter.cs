using Steam_Achievements_Analysis_System.YourOutputDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Steam_Achievements_Analysis_System.Converter
{
    class PercentageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<AchievementPercentage> percentages && percentages.Any())
            {
                return string.Join(", ", percentages.Select(p => $"{p.Percentage:F1}%"));
            }

            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    }


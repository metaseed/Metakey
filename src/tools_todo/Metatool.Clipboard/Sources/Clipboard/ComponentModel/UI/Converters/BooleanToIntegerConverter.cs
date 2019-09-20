﻿using System;
using System.Windows;
using System.Windows.Data;

namespace Clipboard.ComponentModel.UI.Converters
{
    /// <summary>
    /// Convert a <see cref="bool"/> to a specified <see cref="int"/> value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(int))]
    internal class BooleanToIntegerConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value that will be use when the input is true
        /// </summary>
        public int TrueValue { get; set; }

        /// <summary>
        /// Gets or sets a value that will be use when the input is false
        /// </summary>
        public int FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueBool = value as bool?;
            if (valueBool == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (valueBool.Value)
            {
                return TrueValue;
            }
            else
            {
                return FalseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

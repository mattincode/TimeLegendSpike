using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimeLegendSpike.Converters
{
    // Code from http://www.codeproject.com/Articles/286171/MultiBinding-in-Silverlight-5

    /// <summary>
    /// Holds information about an error that occured in MultiBinding conversion.
    /// </summary>
    public class MultiBindingValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBindingValidationError"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public MultiBindingValidationError(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException();
            Exception = exception;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Exception.Message;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimeLegendSpike
{
    public static class TextHelper
    {
        public static double DrawText(UIElementCollection container, Point startPos, double maxWidth, string text, SolidColorBrush fontColorBrush,
                              Orientation orientation = Orientation.Horizontal,
                              HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
                              VerticalAlignment verticalAlignment = VerticalAlignment.Center,
                              double fontSize = 9, 
                              double height = Double.NaN)
        {
            var textBlock = new TextBlock();
            textBlock.Foreground = fontColorBrush;
            textBlock.FontSize = fontSize;
            textBlock.Text = text;
            textBlock.MaxWidth = maxWidth;
            if (!Double.IsNaN(height))
                textBlock.Height = height;
            textBlock.TextTrimming = TextTrimming.WordEllipsis;
            textBlock.SetValue(Canvas.LeftProperty, startPos.X);
            textBlock.SetValue(Canvas.TopProperty, startPos.Y);
            textBlock.HorizontalAlignment = horizontalAlignment;
            textBlock.VerticalAlignment = verticalAlignment;
            if (orientation == Orientation.Vertical)
                textBlock.RenderTransform = new RotateTransform() { Angle = -90 };
            container.Add(textBlock);
            return textBlock.ActualWidth;
        }
    }
}

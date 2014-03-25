using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public static double DrawCircle(UIElementCollection container, Point startPos, SolidColorBrush fillBrush, SolidColorBrush strokeBrush, Size size)
        {
            //startPos.Y += 7;
            var canvas = new Canvas();
            canvas.Width = size.Width;
            canvas.Background = new SolidColorBrush(Colors.White);
            //canvas.IsHitTestVisible = true;
            canvas.Height = size.Height;
            canvas.SetValue(Canvas.LeftProperty, startPos.X);
            canvas.SetValue(Canvas.TopProperty, startPos.Y);
            var circlePos = new Point(size.Width/2, size.Height/2);
            var circle = new Ellipse {Fill = fillBrush, Stroke = strokeBrush};
            circle.SetValue(Canvas.LeftProperty, circlePos.X - 5);
            circle.SetValue(Canvas.TopProperty, circlePos.Y - 5);
            circle.Width = 10;
            circle.Height = 10;
            //circle.IsHitTestVisible = false;
            ToolTipService.SetToolTip(circle, "circle");
            canvas.Children.Add(circle);
            ToolTipService.SetToolTip(canvas, "Helloooo");
            container.Add(canvas);
            return circle.Width;
        }
    }
}

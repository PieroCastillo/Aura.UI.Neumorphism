using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Aura.UI.Neumorphism.Controls 
{
    public enum Direction
    {
        TopLeft = 145,
        TopRight = 225,
        BottomLeft = 315,
        BottomRight = 45
    }

    public enum Shape
    {
        Normal,
        Inset,
        Concave,
        Convex
    }

    public partial class Neumor : Control
    {
        static Neumor()
        {
            AffectsRender<Neumor>(
                BackgroundProperty,
                RadiusProperty,
                DistanceProperty,
                IntensityProperty,
                BlurProperty,
                DirectionProperty,
                ShapeProperty);

            WidthProperty.OverrideDefaultValue<Neumor>(100);
            HeightProperty.OverrideDefaultValue<Neumor>(100);
        }

        public Neumor()
        {
            this.GetObservable(BackgroundProperty).Subscribe(_ => OnColorAffectedPropertyChanged());
            this.GetObservable(IntensityProperty).Subscribe(_ => OnColorAffectedPropertyChanged());
            this.GetObservable(ShapeProperty).Subscribe(_ => OnColorAffectedPropertyChanged());

            this.GetObservable(BoundsProperty).Subscribe(_ => OnAngleOrDistanceAffected());
            this.GetObservable(DirectionProperty).Subscribe(_ => OnAngleOrDistanceAffected());
            this.GetObservable(DistanceProperty).Subscribe(_ => OnAngleOrDistanceAffected());
        }

        Color  dark, light, firstGradient, secondGradient;
        RelativePoint point1, point2;
        (double x, double y, double angle) positions;

        private void OnColorAffectedPropertyChanged()
        {
            var color = Background;

            dark = ChangeColorLuminosity(color, Intensity * -1);
            light = ChangeColorLuminosity(color, Intensity);
            firstGradient = ChangeColorLuminosity(color, Shape == Shape.Convex ? 0.7 : -0.1);
            secondGradient = ChangeColorLuminosity(color, Shape == Shape.Concave ? 0.7 : -0.1);

            Debug.WriteLine(firstGradient);
            Debug.WriteLine(secondGradient);

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual);
        }

        private void OnAngleOrDistanceAffected()
        {
            positions = GetPositionInfo(Distance);

            if(Shape is Shape.Convex)
            {
                point1 = new RelativePoint(1, 0, RelativeUnit.Relative);
                point2 = new RelativePoint(0, 1, RelativeUnit.Relative);
            }
            else
            {
                point1 = RelativePoint.TopLeft;
                point2 = RelativePoint.BottomRight;
            }

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual);
        }

        public override void Render(DrawingContext context)
        {
            IBrush brush;
            bool isInset = Shape == Shape.Inset ? true : false;

            switch (Shape)
            {
                case Shape.Concave:
                    brush = new LinearGradientBrush() 
                    { 
                        GradientStops = new GradientStops { new GradientStop(firstGradient, 0), new GradientStop(secondGradient, 1) },
                        StartPoint = point1,
                        EndPoint = point2,
                        SpreadMethod = GradientSpreadMethod.Pad,
                    };
                    break;
                case Shape.Convex:
                    brush = new LinearGradientBrush()
                    {
                        GradientStops = new GradientStops { new GradientStop(firstGradient, 0), new GradientStop(secondGradient, 1) },
                        StartPoint = point1,
                        EndPoint = point2,
                        SpreadMethod = GradientSpreadMethod.Pad,
                    };
                    break;
                default:
                    brush = new SolidColorBrush(Background);
                    break;
            }

            var boxShadows = new BoxShadows(
                new BoxShadow() { Blur = Blur, Color = light, OffsetX = -positions.x, OffsetY = -positions.y, IsInset = isInset },

                new BoxShadow[] {
                    new BoxShadow() { Blur = Blur, Color = dark, OffsetX = positions.x, OffsetY = positions.y, IsInset = isInset } 
                });

            context.DrawRectangle(brush, null, Bounds.WithoutPosition(), Radius, Radius, boxShadows);
        }

        private (double x, double y, double angle) GetPositionInfo(double distance)
        {
            switch (Direction)
            {
                case Direction.TopLeft:
                    return (distance, distance, 145);
                case Direction.TopRight:
                    return (distance * -1, distance, 225);
                case Direction.BottomLeft:
                    return (distance * -1, distance * -1, 315);
                case Direction.BottomRight:
                    return (distance, distance * -1, 45);

                default:
                    return (distance, distance, 145);
            }
        }

        public static Color ChangeColorLuminosity(Color color, double newluminosityFactor)
        {
            var red = (double)color.R;
            var green = (double)color.G;
            var blue = (double)color.B;

            unchecked
            {
                if (newluminosityFactor < 0)//applies darkness
                {
                    newluminosityFactor++;
                    red *= newluminosityFactor;
                    green *= newluminosityFactor;
                    blue *= newluminosityFactor;
                    //Debug.WriteLine($"dark: {red} {green} {blue}");
                }
                else if (newluminosityFactor >= 0) //applies lightness
                {
                    //newluminosityFactor--;
                    red = (255 - red) * newluminosityFactor + red;
                    green = (255 - green) * newluminosityFactor + green;
                    blue = (255 - blue) * newluminosityFactor + blue;

                    //Debug.WriteLine($"light: {red} {green} {blue}");
                }
                else
                {
                    throw new ArgumentOutOfRangeException("The Luminosity Factor must be a finite number.");
                }

                var c = Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);

                //Debug.WriteLine(c);
                return c;
            }
        }
    }
}

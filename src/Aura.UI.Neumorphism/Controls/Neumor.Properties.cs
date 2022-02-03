using Avalonia;
using Avalonia.Media;

namespace Aura.UI.Neumorphism.Controls
{
    public partial class Neumor
    {
        public Color Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static readonly StyledProperty<Color> BackgroundProperty =
            AvaloniaProperty.Register<Neumor, Color>(nameof(Background), Color.Parse("#FFe0e0e0"));

        public double Radius
        {
            get => GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly StyledProperty<double> RadiusProperty =
            AvaloniaProperty.Register<Neumor, double>(nameof(Radius), 60);

        public double Distance
        {
            get => GetValue(DistanceProperty);
            set => SetValue(DistanceProperty, value);
        }

        public static readonly StyledProperty<double> DistanceProperty =
            AvaloniaProperty.Register<Neumor, double>(nameof(Distance), 20);

        public double Intensity
        {
            get => GetValue(IntensityProperty);
            set => SetValue(IntensityProperty, value);
        }

        public static readonly StyledProperty<double> IntensityProperty =
            AvaloniaProperty.Register<Neumor, double>(nameof(Intensity), 0.15);

        public double Blur
        {
            get => GetValue(BlurProperty);
            set => SetValue(BlurProperty, value);
        }

        public static readonly StyledProperty<double> BlurProperty =
            AvaloniaProperty.Register<Neumor, double>(nameof(Blur), 60);

        public Direction Direction
        {
            get => GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public static readonly StyledProperty<Direction> DirectionProperty =
            AvaloniaProperty.Register<Neumor, Direction>(nameof(Direction), Direction.TopLeft);

        public Shape Shape
        {
            get => GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }

        public static readonly StyledProperty<Shape> ShapeProperty =
            AvaloniaProperty.Register<Neumor, Shape>(nameof(Shape), Shape.Normal);
    }
}

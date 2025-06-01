namespace Capstone.Domain.Common.ValueObjects
{
    public record Point
    {
        public double Value { get; }
        private Point(double value) => Value = value;
        public static Point Of(double value)
        {
            return new Point(value);
        }
    }
}

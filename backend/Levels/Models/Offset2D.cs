namespace Levels.Models;

public class Offset2D
{
    public int X;
    public int Y;

    public Offset2D(int x = 0, int y = 0)
    {
        X = x;
        Y = y;
    }

    public static implicit operator Point(Offset2D o) => new(o.X, o.Y);
    public static explicit operator Offset2D(Point p) => new(p.X, p.Y);

    public static Offset2D operator +(Offset2D o, int n) => new(o.X + n, o.Y + n);
    public static Offset2D operator -(Offset2D o, int n) => new(o.X - n, o.Y - n);

    public override string ToString() => $"({X}, {Y})";
}

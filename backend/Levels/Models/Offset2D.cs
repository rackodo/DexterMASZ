namespace Levels.Models;

public class Offset2D
{
	public int x;
	public int y;

	public Offset2D(int x = 0, int y = 0)
	{
		this.x = x;
		this.y = y;
	}

	public static implicit operator SixLabors.ImageSharp.Point(Offset2D o) => new(o.x, o.y);
	public static explicit operator Offset2D(SixLabors.ImageSharp.Point p) => new(p.X, p.Y);

	public static Offset2D operator +(Offset2D o, int n) => new(o.x + n, o.y + n);
	public static Offset2D operator -(Offset2D o, int n) => new(o.x - n, o.y - n);

	public override string ToString()
	{
		return $"({x}, {y})";
	}
}
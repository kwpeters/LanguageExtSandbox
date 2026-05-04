using System;

namespace LanguageExtLib;
public abstract record Shape
{
    // Protected so nested cases can call the base constructor.
    protected Shape() { }

    /// <summary>
    /// Exhaustive case analysis. Defined as abstract so that each derived
    /// record is forced to implement it.  If a switch expression were used
    /// here, the compiler would not enforce exhaustiveness, and a missing case
    /// would lead to a runtime exception instead of a compile-time error.
    /// </summary>
    public abstract T
    Match<T>(
        Func<Triangle, T> triangleFn,
        Func<Rectangle, T> rectangleFn
    );

    /// <summary>Calculates the area of the shape.</summary>
    public double Area() =>
        Match(
            t => 0.5 * t.Props.BaseCm * t.Props.HeightCm,
            r => r.Props.LengthCm * r.Props.WidthCm);

    //--------------------------------------------------------------------------
    // Cases
    //--------------------------------------------------------------------------

    public sealed record Triangle(TriangleProps Props) : Shape
    {
        public override T
        Match<T>(
            Func<Triangle, T> triangleFn,
            Func<Rectangle, T> rectangleFn
        ) => triangleFn(this);
    }

    public sealed record Rectangle(RectangleProps Props) : Shape
    {
        public override T
        Match<T>(
            Func<Triangle, T> triangleFn,
            Func<Rectangle, T> rectangleFn
        ) => rectangleFn(this);
    }
}


public sealed record TriangleProps
{
    public double BaseCm { get; }
    public double HeightCm { get; }

    private TriangleProps(double baseCm, double heightCm)
    {
        BaseCm = baseCm;
        HeightCm = heightCm;
    }

    public static TriangleProps Create(double baseCm, double heightCm)
    {
        if (baseCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(baseCm), "Base must be positive.");

        if (heightCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(heightCm), "Height must be positive.");

        return new TriangleProps(baseCm, heightCm);
    }
}

public sealed record RectangleProps
{
    public double LengthCm { get; }
    public double WidthCm { get; }

    private RectangleProps(double lengthCm, double widthCm)
    {
        LengthCm = lengthCm;
        WidthCm = widthCm;
    }

    public static RectangleProps Create(double lengthCm, double widthCm)
    {
        if (lengthCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(lengthCm), "Length must be positive.");

        if (widthCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(widthCm), "Width must be positive.");

        return new RectangleProps(lengthCm, widthCm);
    }
}

using LanguageExtLib;

namespace LanguageExtLib.Test;

public class ShapeTests
{
    // ── Construction ─────────────────────────────────────────────────────────

    [Fact]
    public void CanCreateTriangle()
    {
        Shape shape = new Shape.Triangle(TriangleProps.Create(baseCm: 10d, heightCm: 5d));
        Assert.IsType<Shape.Triangle>(shape);
    }

    [Fact]
    public void CanCreateRectangle()
    {
        Shape shape = new Shape.Rectangle(RectangleProps.Create(lengthCm: 8d, widthCm: 3d));
        Assert.IsType<Shape.Rectangle>(shape);
    }

    // ── Match ────────────────────────────────────────────────────────────────

    [Fact]
    public void MatchExtractsTriangleDimensions()
    {
        Shape shape = new Shape.Triangle(TriangleProps.Create(baseCm: 6d, heightCm: 4d));

        var (b, h) = shape.Match(
            triangle  => (triangle.Props.BaseCm, triangle.Props.HeightCm),
            rectangle => throw new InvalidOperationException("Expected Triangle"));

        Assert.Equal(6d, b);
        Assert.Equal(4d, h);
    }

    [Fact]
    public void MatchExtractsRectangleDimensions()
    {
        Shape shape = new Shape.Rectangle(RectangleProps.Create(lengthCm: 9d, widthCm: 2d));

        var (l, w) = shape.Match(
            triangle  => throw new InvalidOperationException("Expected Rectangle"),
            rectangle => (rectangle.Props.LengthCm, rectangle.Props.WidthCm));

        Assert.Equal(9d, l);
        Assert.Equal(2d, w);
    }

    // ── Area ─────────────────────────────────────────────────────────────────

    [Fact]
    public void TriangleAreaIsHalfBaseTimesHeight()
    {
        Shape shape = new Shape.Triangle(TriangleProps.Create(baseCm: 10d, heightCm: 5d));
        Assert.Equal(25d, shape.Area());
    }

    [Fact]
    public void RectangleAreaIsLengthTimesWidth()
    {
        Shape shape = new Shape.Rectangle(RectangleProps.Create(lengthCm: 8d, widthCm: 3d));
        Assert.Equal(24d, shape.Area());
    }

    [Fact]
    public void WithEmptyExpressionCopiesWithoutMutatingDimensions()
    {
        var triangle = new Shape.Triangle(TriangleProps.Create(baseCm: 10d, heightCm: 5d));

        var copy = triangle with { };

        Assert.Equal(triangle, copy);
        Assert.Equal(10d, copy.Props.BaseCm);
        Assert.Equal(5d, copy.Props.HeightCm);
    }

    [Fact]
    public void TriangleCanBeCopiedWithNewValidatedProps()
    {
        var triangle = new Shape.Triangle(TriangleProps.Create(baseCm: 10d, heightCm: 5d));

        var updated = triangle with { Props = TriangleProps.Create(baseCm: 12d, heightCm: 5d) };

        Assert.Equal(10d, triangle.Props.BaseCm);
        Assert.Equal(12d, updated.Props.BaseCm);
        Assert.Equal(5d, updated.Props.HeightCm);
    }

    [Fact]
    public void TrianglePropsCreateRejectsInvalidBase()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TriangleProps.Create(baseCm: -1d, heightCm: 5d));
    }
}

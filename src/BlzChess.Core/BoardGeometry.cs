namespace BlzChess.Core;

/// <summary>
/// Where a point on the drawn board falls among the squares. Directions are the ones
/// the player sees: file a on the left, rank 8 at the top.
/// </summary>
public static class BoardGeometry
{
    /// <summary>
    /// The square under a point, or <see cref="Squares.None"/> when the point is off the
    /// board. <paramref name="x"/> and <paramref name="y"/> are measured from the board's
    /// top-left corner, and <paramref name="side"/> is the board's edge, all in the same
    /// units.
    /// </summary>
    public static int SquareAt(double x, double y, double side)
    {
        if (side <= 0 || double.IsNaN(x) || double.IsNaN(y) || x < 0 || y < 0 || x >= side || y >= side)
        {
            return Squares.None;
        }

        var file = Math.Min(7, (int)(x / (side / 8)));
        var rankFromTop = Math.Min(7, (int)(y / (side / 8)));
        return Squares.Index(file, 7 - rankFromTop);
    }
}
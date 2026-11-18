namespace BlzChess.Core;

/// <summary>
/// The two decisions a drag makes, kept out of the component so they can be tested:
/// which square a press lifts a piece from, and which square a release drops it on.
/// </summary>
public static class Drag
{
    /// <summary>
    /// The square a press lifts a piece from, or <see cref="Squares.None"/> when the
    /// square holds nothing the player can move. The computer's pieces, the wrong side
    /// to move, and a square the player's piece cannot leave all lift nothing.
    /// </summary>
    public static int Lift(GameSession session, int square)
    {
        if (square == Squares.None || session.Result.IsOver || session.IsEngineThinking)
        {
            return Squares.None;
        }

        if (session.Game.Position[square] is not { } piece || piece.Color != session.PlayerColor)
        {
            return Squares.None;
        }

        return session.LegalMovesFrom(square).Count == 0 ? Squares.None : square;
    }

    /// <summary>
    /// The square a release plays a move on, or <see cref="Squares.None"/> when the
    /// release is not on one of the lifted piece's destinations. A release anywhere
    /// else is not a move, and the piece stays where it was.
    /// </summary>
    public static int Land(IReadOnlySet<int> destinations, int square) =>
        square != Squares.None && destinations.Contains(square) ? square : Squares.None;
}
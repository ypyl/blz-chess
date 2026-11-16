using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class MoveGeneratorTests
{
    private const string Kiwipete = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
    private const string PawnEndgame = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1";

    [Theory]
    [InlineData(Fen.StartingPosition, 1, 20)]
    [InlineData(Fen.StartingPosition, 2, 400)]
    [InlineData(Fen.StartingPosition, 3, 8902)]
    [InlineData(Kiwipete, 1, 48)]
    [InlineData(Kiwipete, 2, 2039)]
    [InlineData(PawnEndgame, 1, 14)]
    [InlineData(PawnEndgame, 2, 191)]
    [InlineData(PawnEndgame, 3, 2812)]
    public void PerftMatchesTheKnownCounts(string fen, int depth, long expected)
    {
        Assert.Equal(expected, Perft(Position.FromFen(fen), depth));
    }

    [Fact]
    public void PerftOfTheStartPositionAtDepthFour()
    {
        Assert.Equal(197281, Perft(Position.Initial, 4));
    }

    [Fact]
    public void BothSidesCanCastleFromTheStartOfACastlingPosition()
    {
        var position = Position.FromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
        var moves = MoveGenerator.LegalMoves(position);

        Assert.Contains(new Move(Squares.Index(4, 0), Squares.Index(6, 0)), moves);
        Assert.Contains(new Move(Squares.Index(4, 0), Squares.Index(2, 0)), moves);
        Assert.Equal(26, moves.Count);
    }

    [Fact]
    public void CastlingThroughAnAttackedSquareIsNotLegal()
    {
        var position = Position.FromFen("r3k2r/8/8/8/8/8/6b1/R3K2R w KQkq - 0 1");
        var moves = MoveGenerator.LegalMoves(position);

        Assert.DoesNotContain(new Move(Squares.Index(4, 0), Squares.Index(6, 0)), moves);
        Assert.Contains(new Move(Squares.Index(4, 0), Squares.Index(2, 0)), moves);
    }

    [Fact]
    public void EnPassantCapturesThePawnThatJustMoved()
    {
        var position = Position.FromFen("rnbqkbnr/ppp1p1pp/8/3pPp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3");
        var enPassant = new Move(Squares.Index(4, 4), Squares.Index(5, 5));

        Assert.Contains(enPassant, MoveGenerator.LegalMoves(position));

        var after = position.ApplyUnchecked(enPassant);
        Assert.Null(after[Squares.Index(5, 4)]);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), after[Squares.Index(5, 5)]);
    }

    [Fact]
    public void APawnOnTheLastRankOffersFourPromotions()
    {
        var position = Position.FromFen("8/P6k/8/8/8/8/8/K7 w - - 0 1");
        var promotions = MoveGenerator.LegalMoves(position)
            .Where(move => move.From == Squares.Index(0, 6))
            .Select(move => move.Promotion)
            .ToList();

        Assert.Equal(4, promotions.Count);
        Assert.Contains(PieceType.Queen, promotions);
        Assert.Contains(PieceType.Rook, promotions);
        Assert.Contains(PieceType.Bishop, promotions);
        Assert.Contains(PieceType.Knight, promotions);
    }

    private static long Perft(Position position, int depth)
    {
        var moves = MoveGenerator.LegalMoves(position);
        if (depth == 1)
        {
            return moves.Count;
        }

        long nodes = 0;
        foreach (var move in moves)
        {
            nodes += Perft(position.ApplyUnchecked(move), depth - 1);
        }

        return nodes;
    }
}
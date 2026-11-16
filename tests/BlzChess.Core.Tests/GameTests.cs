using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class GameTests
{
    private const string FoolsMate = "rnb1kbnr/pppp1ppp/8/4p3/6Pq/5P2/PPPPP2P/RNBQKBNR w KQkq - 1 3";
    private const string Stalemate = "7k/5Q2/6K1/8/8/8/8/8 b - - 0 1";
    private const string KingAndBishop = "8/8/4k3/8/8/4K3/8/6B1 w - - 0 1";
    private const string PinnedPawn = "4k3/8/8/8/8/4r3/4P3/4K3 w - - 0 1";
    private const string PawnOnSeventh = "8/P6k/8/8/8/8/8/K7 w - - 0 1";

    [Fact(DisplayName = "Checkmate ends the game")]
    public void CheckmateEndsTheGame()
    {
        var game = new Game(Position.FromFen(FoolsMate));

        Assert.Equal(ResultKind.Checkmate, game.Result.Kind);
        Assert.Equal(Color.Black, game.Result.Winner);
        Assert.Equal("Checkmate. The computer wins.", game.Result.Describe(Color.White));
        Assert.False(game.TryMove(new Move(Squares.Index(4, 1), Squares.Index(4, 2)), out _));
    }

    [Fact(DisplayName = "An illegal move is refused")]
    public void AnIllegalMoveIsRefused()
    {
        var game = new Game();
        Assert.True(game.TryMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        Assert.True(game.TryMove(new Move(Squares.Index(4, 6), Squares.Index(4, 4)), out _));
        var before = game.Fen;

        var moved = game.TryMove(new Move(Squares.Index(4, 3), Squares.Index(4, 5)), out _);

        Assert.False(moved);
        Assert.Equal(before, game.Fen);
        Assert.Equal(Color.White, game.Position.SideToMove);
    }

    [Fact(DisplayName = "An illegal move leaves the king in check")]
    public void AnIllegalMoveLeavesTheKingInCheck()
    {
        var game = new Game(Position.FromFen(PinnedPawn));
        var pinnedPawnPush = new Move(Squares.Index(4, 1), Squares.Index(4, 3));

        Assert.False(game.IsLegal(pinnedPawnPush));
        Assert.False(game.TryMove(pinnedPawnPush, out _));
        Assert.Equal(PinnedPawn, game.Fen);
    }

    [Fact(DisplayName = "A pawn reaching the last rank becomes a queen")]
    public void APawnReachingTheLastRankBecomesAQueen()
    {
        var game = new Game(Position.FromFen(PawnOnSeventh));

        Assert.True(game.TryMove(new Move(Squares.Index(0, 6), Squares.Index(0, 7), PieceType.Queen), out var applied));

        Assert.Equal(PieceType.Queen, applied.Promotion);
        Assert.Equal(new Piece(Color.White, PieceType.Queen), game.Position[Squares.Index(0, 7)]);
    }

    [Fact]
    public void StalemateIsADraw()
    {
        var game = new Game(Position.FromFen(Stalemate));

        Assert.Equal(ResultKind.Stalemate, game.Result.Kind);
        Assert.Null(game.Result.Winner);
    }

    [Fact]
    public void InsufficientMaterialIsADraw()
    {
        var game = new Game(Position.FromFen(KingAndBishop));

        Assert.Equal(ResultKind.InsufficientMaterial, game.Result.Kind);
    }

    [Fact]
    public void TheFiftyMoveRuleIsADraw()
    {
        var game = new Game(Position.FromFen("8/8/4k3/8/8/4K3/8/6B1 w - - 100 80"));

        Assert.Equal(ResultKind.FiftyMove, game.Result.Kind);
    }

    [Fact]
    public void ThreefoldRepetitionIsADraw()
    {
        var game = new Game();
        var shuffle = new[]
        {
            new Move(Squares.Index(6, 0), Squares.Index(5, 2)),
            new Move(Squares.Index(6, 7), Squares.Index(5, 5)),
            new Move(Squares.Index(5, 2), Squares.Index(6, 0)),
            new Move(Squares.Index(5, 5), Squares.Index(6, 7)),
        };

        for (var round = 0; round < 2; round++)
        {
            foreach (var move in shuffle)
            {
                Assert.True(game.TryMove(move, out _));
            }
        }

        Assert.Equal(ResultKind.ThreefoldRepetition, game.Result.Kind);
    }

    [Fact]
    public void TheStartingPositionIsNotOver()
    {
        var game = new Game();

        Assert.Equal(ResultKind.InProgress, game.Result.Kind);
        Assert.Equal("Your move.", game.Result.Describe(Color.White));
        Assert.Equal(20, game.LegalMoves().Count);
    }
}
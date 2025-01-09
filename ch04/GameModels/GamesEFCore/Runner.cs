using GameModels;

namespace GamesEFCore;

internal class Runner(GamesContext context)
{
    public async Task CreateDatabaseAsync()
    {
        // await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task CreateAndStoreGame1Async()
    {
        ColorGame game = new("player1") 
        { 
            Solution = ["red", "green", "blue", "yellow"] 
        };
        ColorMove move = new(1)
        {
            Colors = [ "violet", "orange", "green", "red"]
        };
        ColorResult result = game.SetMove(move);

        context.Games.Add(game);
        await context.SaveChangesAsync();
    }

    public async Task CreateAndStoreGame2Async()
    {
        ShapeAndColorGame game = new("player2")
        {
            Solution = [("circle", "red"), ("square", "green"), ("triangle", "blue"), ("star", "yellow")]
        };
        ShapeAndColorMove move = new(1)
        {
            ShapesAndColors = [("circle", "red"), ("triangle", "blue"), ("triangle", "green"), ("star", "blue")]
        };
        ShapeAndColorResult result = game.SetMove(move);

        context.Games.Add(game);
        await context.SaveChangesAsync();
    }
}

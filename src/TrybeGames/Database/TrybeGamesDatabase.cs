namespace TrybeGames;

public class TrybeGamesDatabase
{
    public List<Game> Games = new List<Game>();

    public List<GameStudio> GameStudios = new List<GameStudio>();

    public List<Player> Players = new List<Player>();

    // 4. Crie a funcionalidade de buscar jogos desenvolvidos por um estúdio de jogos
    public List<Game> GetGamesDevelopedBy(GameStudio gameStudio)
    {
        // implementar
        var games = from game in Games
                    where game.DeveloperStudio == gameStudio.Id
                    select game;
        return games.ToList();
    }

    // 5. Crie a funcionalidade de buscar jogos jogados por uma pessoa jogadora
    public List<Game> GetGamesPlayedBy(Player player)
    {
        // Implementar
        var games = from game in Games
                    where game.Players.Contains(player.Id)
                    select game;
        return games.ToList(); 
    }

    // 6. Crie a funcionalidade de buscar jogos comprados por uma pessoa jogadora
    public List<Game> GetGamesOwnedBy(Player playerEntry)
    {
        // Implementar
        var games = from game in Games
                    where playerEntry.GamesOwned.Contains(game.Id)
                    select game;
        return games.ToList();
    }


    // 7. Crie a funcionalidade de buscar todos os jogos junto do nome do estúdio desenvolvedor
    public List<GameWithStudio> GetGamesWithStudio()
    {
        // Implementar
        var games = from game in Games
                    join studio in GameStudios on game.DeveloperStudio equals studio.Id
                    select new GameWithStudio {
                        GameName = game.Name,
                        StudioName = studio.Name,
                        NumberOfPlayers = Players.Count(player => player.GamesOwned.Contains(game.Id))
                    };
        return games.ToList();
    }
    
    // 8. Crie a funcionalidade de buscar todos os diferentes Tipos de jogos dentre os jogos cadastrados
    public List<GameType> GetGameTypes()
    {
        // Implementar
        var gamesType = from game in Games
                        select game.GameType;
        return gamesType.Distinct().ToList();
    }

    // 9. Crie a funcionalidade de buscar todos os estúdios de jogos junto dos seus jogos desenvolvidos com suas pessoas jogadoras
    public List<StudioGamesPlayers> GetStudiosWithGamesAndPlayers()
    {
        // Implementar
        var allInfos = (
            from studio in GameStudios
            select new StudioGamesPlayers
            {
                GameStudioName = studio.Name,
                Games = (
                    from game in Games
                    where game.DeveloperStudio == studio.Id
                    select new GamePlayer
                    {
                        GameName = game.Name,
                        Players = (from player in Players
                            where player.GamesOwned.Contains(game.Id)
                            select player).ToList()
                    }).ToList()
            }).ToList();

        return allInfos;
    }

}

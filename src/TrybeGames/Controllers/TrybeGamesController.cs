using System.Globalization;

namespace TrybeGames;
public class TrybeGamesController
{
    public TrybeGamesDatabase database;

    public IConsole Console;

    public TrybeGamesController(TrybeGamesDatabase database, IConsole console)
    {
        this.database = database;
        this.Console = console;
    }

    public void RemovePlayerFromGame(Game game)
    {
        var playersPlayingGame = database.Players.Where(p => game.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrado!");
            return;
        }
        game.RemovePlayer(player);
        Console.WriteLine("Pessoa jogadora removido com sucesso!");
    }

    public void AddPlayerToGame(Game gameToAdd)
    {
        var playersNotPlayingGame = database.Players.Where(p => !gameToAdd.Players.Contains(p.Id)).ToList();
        var player = SelectPlayer(playersNotPlayingGame);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        gameToAdd.AddPlayer(player);
        Console.WriteLine("Pessoa jogadora adicionada com sucesso!");
    }

    public void QueryGamesFromStudio()
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        try {
            var games = database.GetGamesDevelopedBy(gameStudio);
            Console.WriteLine("Jogos do estúdio de jogos " + gameStudio.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    public void QueryGamesPlayedByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try {
            var games = database.GetGamesPlayedBy(player);
            if (games.Count() == 0)
            {
                Console.WriteLine("Pessoa jogadora não jogou nenhum jogo!");
                return;
            }
            Console.WriteLine("Jogos jogados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }

    }

    public void QueryGamesBoughtByPlayer()
    {
        var player = SelectPlayer(database.Players);
        if (player == null)
        {
            Console.WriteLine("Pessoa jogadora não encontrada!");
            return;
        }
        try {
            var games = database.GetGamesOwnedBy(player);
            Console.WriteLine("Jogos comprados pela pessoa jogadora " + player.Name + ":");
            foreach (var game in games)
            {
                Console.WriteLine(game.Name);
            }
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Ainda não é possível realizar essa funcionalidade!");
            return;
        }
    }

    // 1. Crie a funcionalidde para adicionar uma nova pessoa jogadora ao banco de dados
    public void AddPlayer()
    {
        // implementar
        Console.WriteLine("Digite o nome do novo usuario:");
        string name = Console.ReadLine();
        Player newPlayer = new Player { Id = database.Players.Count + 1, Name = name};
        database.Players.Add(newPlayer);
        var gamesType = from game in database.Games
                        select game.GameType;
        Console.WriteLine(string.Join(", ", gamesType));
    }

    // 2. Crie a funcionalidade de adicionar um novo estúdio de jogos ao banco de dados
    public void AddGameStudio()
    {
        // implementar
        Console.WriteLine("Digite o nome do novo estudio:");
        string studio = Console.ReadLine();
        GameStudio newStudio = new GameStudio { Id = database.GameStudios.Count + 1, Name = studio};
        database.GameStudios.Add(newStudio);
    }

    // 3. Crie a funcionalidade de adicionar novo Jogo ao Banco de dados
    public void AddGame()
    {
        // implementar
        Console.WriteLine("Digite o nome do novo jogo:");
        string game = Console.ReadLine();
        Console.WriteLine("Digite a data de lançamento do novo jogo:");
        DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        Console.WriteLine("Digite o tipo do novo jogo:");
        string type = Console.ReadLine();
        Game newGame = new Game { Id = database.Games.Count + 1, Name = game, ReleaseDate = date, GameType = (GameType)int.Parse(type)};
        database.Games.Add(newGame);
    }

    public void ChangeGameStudio(Game game)
    {
        var gameStudio = SelectGameStudio(database.GameStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Opção inválida! Tente novamente.");
            return;
        }
        game.DeveloperStudio = gameStudio.Id;
        Console.WriteLine("Estúdio de jogos alterado com sucesso!");
    }

    public void AddGameStudioToFavorites(Player player)
    {
        var notFavoriteStudios = database.GameStudios.Where(s => !player.FavoriteGameStudios.Contains(s.Id)).ToList();
        var gameStudio = SelectGameStudio(notFavoriteStudios);
        if (gameStudio == null)
        {
            Console.WriteLine("Nenhum estúdio de jogos encontrado!");
            return;
        }
        player.AddGameStudioToFavorites(gameStudio);
    }

    public void BuyGame(Player player)
    {
        var gamesNotOwned = database.Games.Where(g => !player.GamesOwned.Contains(g.Id)).ToList();
        var game = SelectGame(gamesNotOwned);
        if (game == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            return;
        }
        player.BuyGame(game);
        Console.WriteLine("Jogo comprado com sucesso!");
    }

    public Player SelectPlayer(List<Player> players)
    {
        Console.WriteLine("Selecione o jogador:");
        PrintPlayers(players);
        var playerId = int.Parse(Console.ReadLine() ?? "0");
        Player? result = players.FirstOrDefault(p => p.Id == playerId);
        return result!;
    }

    public Game SelectGame(List<Game> games)
    {
        Console.WriteLine("Selecione o jogo:");
        PrintGames(games);
        var gameId = int.Parse(Console.ReadLine() ?? "0");
        return games.FirstOrDefault(g => g.Id == gameId)!;
    }

    public GameStudio SelectGameStudio(List<GameStudio> gameStudios)
    {
        Console.WriteLine("Selecione o estúdio de jogos:");
        PrintGameStudios(gameStudios);
        var gameStudioId = int.Parse(Console.ReadLine() ?? "0");
        return gameStudios.FirstOrDefault(gs => gs.Id == gameStudioId)!;
    }

    public void PrintGames(List<Game> games)
    {
        foreach (var game in games)
        {
            Console.WriteLine(game.Id + " - " + game.Name);
        }
    }

    public void PrintGameStudios(List<GameStudio> gameStudios)
    {
        foreach (var gameStudio in gameStudios)
        {
            Console.WriteLine(gameStudio.Id + " - " + gameStudio.Name);
        }
    }

    public void PrintPlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            Console.WriteLine(player.Id + " - " + player.Name);
        }
    }

    public void PrintGameTypes()
    {
        Console.WriteLine("1 - Ação");
        Console.WriteLine("2 - Aventura");
        Console.WriteLine("3 - Puzzle");
        Console.WriteLine("4 - Estratégia");
        Console.WriteLine("5 - Simulação");
        Console.WriteLine("6 - Esportes");
        Console.WriteLine("7 - Outro");
    }
}
using ApiCatalogoGames.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoGames.Repositories
{
    public class GameRepository : IGameRepository
    {
         private static Dictionary<Guid, Game> games = new Dictionary<Guid, Game>()
        {
            {Guid.Parse("0fa314a5-9282-45d8-92c3-2985f2a9fd22"), new Game{ Id = Guid.Parse("0fa314a5-9282-45d8-92c3-2985f2a9fd22"), Name = "Cities Skylines", Produtora = "Paradox", Valor = 79.90} },
            {Guid.Parse("eb90aced-1862-478e-8641-1bba36c23db3"), new Game{ Id = Guid.Parse("eb90aced-1862-478e-8641-1bba36c23db3"), Name = "Kindom Rush: Vengeance", Produtora = "Iron Turtle", Valor = 32} },
            {Guid.Parse("5ebcc84a-108b-4dfa-ab7e-d8c55957a71c"), new Game{ Id = Guid.Parse("5ebcc84a-108b-4dfa-ab7e-d8c55957a71c"), Name = "Left 4 Dead 2", Produtora = "Valve", Valor = 22.70} },
            {Guid.Parse("15033439-f352-4539-879f-ee5759312d53"), new Game{ Id = Guid.Parse("15033439-f352-4539-879f-ee5759312d53"), Name = "The Elder Scrolls: Skyrim", Produtora = "Bethesda", Valor = 27} },
            {Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), new Game{ Id = Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), Name = "Street Fighter V", Produtora = "Capcom", Valor = 32.90} },
            {Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), new Game{ Id = Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), Name = "Grand Theft Auto V", Produtora = "Rockstar Games", Valor = 64.90} }
        };

        public Task<List<Game>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(games.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Game> Obter(Guid id)
        {
            if (!games.ContainsKey(id))
                return Task.FromResult<Game>(null);

            return Task.FromResult(games[id]);
        }

        public Task<List<Game>> Obter(string nome, string produtora)
        {
            return Task.FromResult(games.Values.Where(game => game.Name.Equals(nome) && game.Produtora.Equals(produtora)).ToList());
        }

        public Task<List<Game>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Game>();

            foreach(var game in games.Values)
            {
                if (game.Name.Equals(nome) && game.Produtora.Equals(produtora))
                    retorno.Add(game);
            }

            return Task.FromResult(retorno);
        }

        public Task Inserir(Game game)
        {
            games.Add(game.Id, game);
            return Task.CompletedTask;
        }

        public Task Atualizar(Game game)
        {
            games[game.Id] = game;
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            games.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão com o banco. Sem implementação por não ter conexão com um banco de dados real.
        }
    }
}

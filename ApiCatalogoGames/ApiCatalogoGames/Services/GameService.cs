using ApiCatalogoGames.Entities;
using ApiCatalogoGames.Exceptions;
using ApiCatalogoGames.InputModel;
using ApiCatalogoGames.Repositories;
using ApiCatalogoGames.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoGames.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Obter(int pagina, int quantidade)
        {
            var games = await _gameRepository.Obter(pagina, quantidade);
            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Produtora = game.Produtora,
                Valor = game.Valor
            }).ToList();
        }

        public async Task<GameViewModel> Obter(Guid id)
        {
            var game = await _gameRepository.Obter(id);
            if (game == null)
            {
                return null;
            }
            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Produtora = game.Produtora,
                Valor = game.Valor
            };
        }

        public async Task<GameViewModel> Inserir(GameInputModel game)
        {
            var entidadeGame = await _gameRepository.Obter(game.Nome, game.Produtora);
            if (entidadeGame.Count > 0)
            {
                throw new GameJaCadastradoException();
            }
            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Nome,
                Produtora = game.Produtora,
                Valor = game.Valor
            };

            await _gameRepository.Inserir(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Nome,
                Produtora = game.Produtora,
                Valor = game.Valor
            };
        }

        public async Task Atualizar(Guid id , GameInputModel game)
        {
            var entidadeGame = await _gameRepository.Obter(id);
            if (entidadeGame == null)
            {
                throw new GameNaoCadastradoException();
            }
            entidadeGame.Name = game.Nome;
            entidadeGame.Produtora = game.Produtora;
            entidadeGame.Valor = game.Valor;

            await _gameRepository.Atualizar(entidadeGame);
        }

        public async Task Atualizar(Guid id, double valor)
        {
            var entidadeGame = await _gameRepository.Obter(id);
            if (entidadeGame == null)
            {
                throw new GameNaoCadastradoException();
            }
            entidadeGame.Valor = valor;

            await _gameRepository.Atualizar(entidadeGame);
        }

        public async Task Remover(Guid id)
        {
            var game = await _gameRepository.Obter(id);
            if (game == null)
            {
                throw new GameNaoCadastradoException();
            }

            await _gameRepository.Remover(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}

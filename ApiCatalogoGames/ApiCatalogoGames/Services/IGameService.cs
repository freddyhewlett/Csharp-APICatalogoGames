using ApiCatalogoGames.InputModel;
using ApiCatalogoGames.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoGames.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Obter(int pagina, int quantidade);
        Task<GameViewModel> Obter(Guid id);
        Task<GameViewModel> Inserir(GameInputModel game);
        Task Atualizar(Guid id, GameInputModel game);
        Task Atualizar(Guid id, double valor);
        Task Remover(Guid id);
    }
}

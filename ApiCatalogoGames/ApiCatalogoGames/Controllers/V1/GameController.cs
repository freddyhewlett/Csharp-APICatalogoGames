using ApiCatalogoGames.Exceptions;
using ApiCatalogoGames.InputModel;
using ApiCatalogoGames.Services;
using ApiCatalogoGames.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoGames.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de reistros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            throw new Exception();
            var games = await _gameService.Obter(pagina, quantidade);
            if (games.Count() == 0)
            {
                return NoContent();
            }
            
            return Ok(games);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idGame">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com este id</response>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> Obter([FromRoute]Guid idGame)
        {
            var game = await _gameService.Obter(idGame);
            if (game == null)
            {
                return NoContent();
            }

            return Ok(game);
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="gameInputModel">Dados do jogo a ser inserido</param>
        /// <response code="200">Cao o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora</response>
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InserirGame([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.Inserir(gameInputModel);
                return Ok();
            }
            catch (GameJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo
        /// </summary>
        /// /// <param name="idGame">Id do jogo a ser atualizado</param>
        /// <param name="gameInputModel">Novos dados para atualizar o jogo indicado</param>
        /// <response code="200">Cao o jogo seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> AtualizarGame([FromRoute] Guid idGame, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Atualizar(idGame, gameInputModel);
                return Ok();
            }
            catch (GameNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo
        /// </summary>
        /// /// <param name="idGame">Id do jogo a ser atualizado</param>
        /// <param name="valor">Novo preço do jogo</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>
        [HttpPatch("{idGame:guid}/valor/{valor:double}")]
        public async Task<ActionResult<GameViewModel>> AtualizarGame([FromRoute] Guid idGame, [FromRoute] double valor)
        {
            try
            {
                await _gameService.Atualizar(idGame, valor);
                return Ok();
            }
            catch (GameNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        /// <summary>
        /// Excluir um jogo
        /// </summary>
        /// /// <param name="idGame">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> DeletarGame([FromRoute]Guid idGame)
        {
            try
            {
                await _gameService.Remover(idGame);
                return Ok();
            }
            catch (GameNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }
    }
}

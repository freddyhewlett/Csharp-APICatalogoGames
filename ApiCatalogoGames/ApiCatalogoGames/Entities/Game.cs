using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoGames.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Produtora { get; set; }
        public double Valor { get; set; }
    }
}

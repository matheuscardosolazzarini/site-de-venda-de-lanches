using LanchesTeste.Context;
using LanchesTeste.Models;

namespace LanchesTeste.Areas.Admin.Servicos
{
    public class GraficosVendasService
    {
        private readonly AppDbContext context;

        public GraficosVendasService(AppDbContext context)
        {
            this.context = context;
        }

        public List<LancheGrafico>GetVendasLanches(int dias = 360)
        {
            var data = DateTime.Now.AddDays(-dias);

            var lanches = (from pd in context.PedidoDetalhes join l in context.lanches on pd.LancheId equals l.LancheId where pd.Pedido.PedidoEnviado >= data group pd by new { pd.LancheId, l.Nome}
            into g
                           select new
                           {
                               LancheNome = g.Key.Nome,
                               lanchesQuantidade = g.Sum(q => q.Quantidade),
                               LanchesValorTotal = g.Sum(a => a.Preco * a.Quantidade)
                           });

            var lista = new List<LancheGrafico>();

            foreach(var item in lanches)
            {
                var lanche = new LancheGrafico();
                lanche.LancheNome = item.LancheNome;
                lanche.LanchesQuantidade = item.lanchesQuantidade;
                lanche.LanchesValorTotal = item.LanchesValorTotal;
                lista.Add(lanche);
            }
            return lista;
        }
    }
}

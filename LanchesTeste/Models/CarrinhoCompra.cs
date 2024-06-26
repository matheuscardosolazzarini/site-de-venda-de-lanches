﻿using LanchesTeste.Context;
using Microsoft.EntityFrameworkCore;

namespace LanchesTeste.Models
{
	public class CarrinhoCompra
	{
		private readonly AppDbContext _context;

		public CarrinhoCompra(AppDbContext context)
		{
			_context = context;
		}

		public string CarrinhoCompraId { get; set; }

		public List<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }

		public static CarrinhoCompra GetCarrinho(IServiceProvider services)
		{
			//defina uma sessão
			ISession session =
				services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

			//obter um serviço do tipo do nosso contexto
			var context = services.GetService<AppDbContext>();

			//obter ou gerar o ID do carrinho
			string carrinhoId = session.GetString("CarrinhoId")?? Guid.NewGuid().ToString();

			//atribui o id do carrinho na sessão
			session.SetString("CarrinhoId", carrinhoId);

			//retorna o carrinho com o contexto e o id atribuido ou obtido
			return new CarrinhoCompra(context)
			{
				CarrinhoCompraId = carrinhoId
			};
		}

		public void AdicionarAoCarrinho(Lanche lanche)
		{
			var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
				s => s.Lanche.LancheId ==lanche.LancheId && 
				s.CarrinhoCompraId == CarrinhoCompraId);

			if( carrinhoCompraItem == null)
			{
				carrinhoCompraItem = new CarrinhoCompraItem
				{
					CarrinhoCompraId = CarrinhoCompraId,
					Lanche = lanche,
					Quantidade = 1
				};
				_context.CarrinhoCompraItens.Add(carrinhoCompraItem);
			}
			else
			{
				carrinhoCompraItem.Quantidade++;
			}
			_context.SaveChanges();
		}
		public int RemoverDoCarrinho(Lanche lanche)
		{
			var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
				s => s.Lanche.LancheId == lanche.LancheId &&
				s.CarrinhoCompraId == CarrinhoCompraId);

			var quantidadelocal = 0;

			if( carrinhoCompraItem != null)
			{
				if(carrinhoCompraItem.Quantidade > 1)
				{
					carrinhoCompraItem.Quantidade--;
					quantidadelocal = carrinhoCompraItem.Quantidade;
				}
				else
				{
					_context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
				}
			}
			_context.SaveChanges();
			return quantidadelocal;
		}

		public List<CarrinhoCompraItem>GetCarrinhoCompraItems()
		{
			return CarrinhoCompraItems ??
				(CarrinhoCompraItems =
				_context.CarrinhoCompraItens.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
				.Include(s => s.Lanche)
				.ToList());
		}

		public  void LimparCarrinho()
		{
			var carrinhoItems = _context.CarrinhoCompraItens
				.Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);

			_context.CarrinhoCompraItens.RemoveRange(carrinhoItems);
			_context.SaveChanges();
		}

		public decimal GetCarrinhoCompraTotal()
		{
			var total = _context.CarrinhoCompraItens
				.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
				.Select(c => c.Lanche.Preco * c.Quantidade).Sum();

			return total;
		}
	}
}

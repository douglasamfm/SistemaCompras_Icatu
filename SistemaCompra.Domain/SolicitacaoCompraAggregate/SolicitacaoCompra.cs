using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }

        public CondicaoPagamento CondicaoPagamento { get; private set; }

        private const int CondicaoPgto30Dias = 30;

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            TotalGeral = new Money(0);
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            if (Itens == null) Itens = new List<Item>();

            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
            TotalGeral = new Money(0);

            if (!itens.Any())
                throw new BusinessRuleException("A solicitação de compra deve possuir itens!");

            foreach (var item in itens)
                TotalGeral = TotalGeral.Add(item.Subtotal);

            if (TotalGeral.Value > 50000)
                CondicaoPagamento = new CondicaoPagamento(CondicaoPgto30Dias);

            AddEvent(new CompraRegistradaEvent(Id, itens, TotalGeral.Value));

        }
    }
}

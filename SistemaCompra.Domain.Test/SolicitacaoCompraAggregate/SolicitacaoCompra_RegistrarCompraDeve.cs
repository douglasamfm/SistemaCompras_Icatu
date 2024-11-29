using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace SistemaCompra.Domain.Test.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra_RegistrarCompraDeve
    {
        [Fact]
        public void DefinirPrazo30DiasAoComprarMais50mil()
        {
            //Dado
            var solicitacao = new SolicitacaoCompra("rodrigoasth", "rodrigoasth");
     
            var itens = new List<Item>();
            var produto = new Produto("Cedro", "Transversal 3/3", Categoria.Madeira.ToString(), 1001);
            itens.Add(new Item(produto, 50));

            //Quando
            solicitacao.RegistrarCompra(itens);

            //Então
            Assert.Equal(30, solicitacao.CondicaoPagamento.Valor);
        }

        [Fact]
        public void NotificarErroQuandoNaoInformarItensCompra()
        {
            //Dado
            var solicitacao = new SolicitacaoCompra("rodrigoasth", "rodrigoasth");
            var itens = new List<Item>();

            //Quando 
            var ex = Assert.Throws<BusinessRuleException>(() => solicitacao.RegistrarCompra(itens));

            //Então
            Assert.Equal("A solicitação de compra deve possuir itens!", ex.Message);
        }

        [Fact]
        public void QuandoNomeForNuloOuVazio_DeveLancarArgumentNullException()
        {
            // Quando
            var ex1 = Assert.Throws<ArgumentNullException>(() => new UsuarioSolicitante(null));
            var ex2 = Assert.Throws<ArgumentNullException>(() => new UsuarioSolicitante(string.Empty));

            // Então
            Assert.Equal("nome", ex1.ParamName);
            Assert.Equal("nome", ex2.ParamName);
        }

        [Fact]
        public void QuandoNomeForMenorQue5Caracteres_DeveLancarBusinessRuleException()
        {
            // Quando
            var ex = Assert.Throws<BusinessRuleException>(() => new UsuarioSolicitante("Joao"));

            // Então
            Assert.Equal("Nome de usuário deve possuir pelo menos 8 caracteres.", ex.Message);
        }

        [Fact]
        public void QuandoNomeEValido()
        {
            // Dado
            string nomeValido = "DouglasAFernandes";

            // Quando
            var usuario = new UsuarioSolicitante(nomeValido);

            // Então
            Assert.Equal(nomeValido, usuario.Nome);
        }

    }
}

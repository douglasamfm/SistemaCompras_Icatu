using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;

namespace SistemaCompra.API.SolicitacaoCompra
{
    public class SolicitacaoCompraController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitacaoCompraController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public NomeFornecedor NomeFornecedor { get; private set; }

        [HttpPost, Route("solicitacaoCompra/solicitar")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult SolicitarCompra([FromBody] RegistrarCompraCommand registrarCompraCommand)
        {
            var retorno = new Retorno();

            retorno =  ValidaNomeFornecedor(registrarCompraCommand.Fornecedor);

            if (retorno.Sucesso)
            {
                _mediator.Send(registrarCompraCommand);
                return Ok();
            }
            else            
             return BadRequest(retorno.Erros);           

        }

        public Retorno ValidaNomeFornecedor(string nome)
        {
            var retorno = new Retorno();

            if (String.IsNullOrWhiteSpace(nome)) throw new ArgumentNullException(nameof(nome));
            if (nome.Length < 10)
            {
                retorno.Sucesso = false;
                retorno.Erros.Add("Nome de fornecedor deve ter pelo menos 10 caracteres.");
            }
            else
                retorno.Sucesso = true;


            return retorno;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCompra.Domain.Core.Model
{
    public class Retorno
    {
       public Retorno()
       {

            Erros = new List<string>(); 

       }

        public bool Sucesso { get; set; }
        public List<string> Erros { get; set; }
    }
}

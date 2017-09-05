using Imposto.Business;
using Imposto.Domain;
using System.Collections.Generic;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        /// <summary>
        /// Metodo exposto responsavel por gerar a nota fiscal
        /// </summary>
        /// <param name="pedido">Pedido</param>
        public void GerarNotaFiscal(Pedido pedido)
        {            
            NotaFiscalBusiness business = new NotaFiscalBusiness();
            business.EmitirNotaFiscal(pedido);
        }

        /// <summary>
        /// Metodo responsavel por retornar o agrupamento de bases e valores de ICMS e IPI por CFOP
        /// </summary>
        /// <returns>Lista encontrada</returns>
        public List<AgrupamentoCFOP> ConsultarAgrupamentoCFOP()
        {
            NotaFiscalBusiness business = new NotaFiscalBusiness();
            return business.ConsultarAgrupamentoCFOP();           
        }
    }
}

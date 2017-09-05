using System;

namespace Imposto.Domain
{
    public class NotaFiscalItem
    {
        public NotaFiscalItem()
        {
            Icms = new ICMS();
            Ipi = new IPI();
        }

        public int Id { get; set; }

        public int IdNotaFiscal { get; set; }

        public string Cfop { get; set; }        

        public string NomeProduto { get; set; }

        public string CodigoProduto { get; set; }

        public ICMS Icms { get; set; }

        public IPI Ipi { get; set; }    
        
        public Double Desconto { get; set; }
    }
}

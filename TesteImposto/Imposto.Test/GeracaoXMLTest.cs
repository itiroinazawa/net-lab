using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imposto.Helpers;
using Imposto.Domain;

namespace Imposto.Test
{
    [TestClass]
    public class GeracaoXMLTest
    {
        [TestMethod]
        public void GerarXML_Success_Test()
        {
            GeradorXML gerador = new GeradorXML();

            string path_ = @"c:\data\";
            NotaFiscal notaFiscal_ = GerarNotaFiscal();

            bool gerado = gerador.GerarXML(path_, notaFiscal_);

            Assert.IsTrue(gerado);
        }

        [TestMethod]
        public void GerarXML_Fail_Caminho_Test()
        {
            GeradorXML gerador = new GeradorXML();

            string path_ = string.Empty;
            NotaFiscal notaFiscal_ = GerarNotaFiscal();

            bool gerado = gerador.GerarXML(path_, notaFiscal_);

            Assert.IsFalse(gerado);
        }

        [TestMethod]
        public void GerarXML_Fail_SemNotaFiscal_Test()
        {
            GeradorXML gerador = new GeradorXML();

            string path_ = @"c:\data\";
            NotaFiscal notaFiscal_ = null;

            bool gerado = gerador.GerarXML(path_, notaFiscal_);

            Assert.IsFalse(gerado);
        }


        private NotaFiscal GerarNotaFiscal()
        {
            NotaFiscal notaFiscal_ = new NotaFiscal();

            notaFiscal_.EstadoDestino = "MG";
            notaFiscal_.EstadoOrigem = "SP";
            notaFiscal_.NomeCliente = "Teste";
            notaFiscal_.NumeroNotaFiscal = 9999;
            notaFiscal_.Serie = new Random().Next(Int32.MaxValue);
            
            notaFiscal_.ItensDaNotaFiscal.Add(new NotaFiscalItem()
            {
                Cfop = "6.000",
                CodigoProduto = "123",
                Desconto = 0,                
                Icms = new ICMS()
                {
                    AliquotaIcms = 0,
                    BaseIcms = 10,
                    TipoIcms = Constantes.ICMS.TIPO_ICMS_ESTADO_ORIGEM_DESTINO_DIFERENTES,
                    ValorIcms = 1000
                },
                Ipi = new IPI
                {
                    AliquotaIPI = 0.1,
                    BaseIPI = 10000,
                    ValorIPI = 200
                },
                NomeProduto = "Produto Teste"
            });

            return notaFiscal_;
        }
    }
}

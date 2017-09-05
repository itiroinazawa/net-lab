using Imposto.DAL;
using Imposto.Domain;
using Imposto.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Imposto.Business
{
    public class NotaFiscalBusiness
    {
        #region Metodos Publicos

        /// <summary>
        /// Metodo responsavel por emitir a nota fiscal
        /// </summary>
        /// <param name="pedido">Pedido</param>
        public void EmitirNotaFiscal(Pedido pedido)
        {
            NotaFiscal notaFiscal = new NotaFiscal();

            notaFiscal.NumeroNotaFiscal = 99999;
            notaFiscal.Serie = new Random().Next(Int32.MaxValue);
            notaFiscal.NomeCliente = pedido.NomeCliente;

            notaFiscal.EstadoDestino = pedido.EstadoDestino;
            notaFiscal.EstadoOrigem = pedido.EstadoOrigem;

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = GerarNotaFiscalItem(notaFiscal, itemPedido);
                notaFiscal.ItensDaNotaFiscal.Add(notaFiscalItem);
            }

            string path = ConfigurationManager.AppSettings["CaminhoXML"].ToString();

            GeradorXML gerador = new GeradorXML();
            bool xmlGerado = gerador.GerarXML(path, notaFiscal);

            if(xmlGerado)
            { 
                PersistirNotaFiscal(notaFiscal);
            }
        }

        /// <summary>
        /// Metodo responsavel por consultar o agrupamento de valores e base de ICMS e IPI por CFOP
        /// </summary>
        /// <returns>Lista preenchida</returns>
        public List<AgrupamentoCFOP> ConsultarAgrupamentoCFOP()
        {
            NotaFiscalItemRepository db = new NotaFiscalItemRepository();
            List<AgrupamentoCFOP> lista = db.ConsultarAgrupamentoCFOP();

            return lista;
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Metodo responsavel pela persistencia da nota fiscal na base de dados
        /// </summary>
        /// <param name="notaFiscal">Nota Fiscal preenchida</param>
        private void PersistirNotaFiscal(NotaFiscal notaFiscal)
        {
            NotaFiscalRepository db = new NotaFiscalRepository();

            int idNotaFiscal = db.CreateUpdateNotaFiscal(notaFiscal.Id, notaFiscal.NumeroNotaFiscal, notaFiscal.Serie, notaFiscal.NomeCliente, notaFiscal.EstadoDestino, notaFiscal.EstadoOrigem);

            if (idNotaFiscal > 0)
            {
                NotaFiscalItemRepository dbItem = new NotaFiscalItemRepository();

                foreach (var item in notaFiscal.ItensDaNotaFiscal)
                {
                    dbItem.CreateUpdateNotaFiscalItem(item.Id, idNotaFiscal, item.Cfop, 
                                                    item.Icms.TipoIcms, item.Icms.BaseIcms, item.Icms.AliquotaIcms, item.Icms.ValorIcms, 
                                                    item.NomeProduto, item.CodigoProduto, item.Ipi.BaseIPI, item.Ipi.AliquotaIPI, item.Ipi.ValorIPI);
                }
            }
        }

        /// <summary>
        /// Metodo responsavel por verificar se haverá o preenchimento do desconto para o Sudeste
        /// </summary>
        /// <param name="estadoDestino_">Estado de Destino</param>
        /// <returns>Resultado</returns>
        private bool PreencherDescontoSudeste(string estadoDestino_)
        {
            return estadoDestino_ == Constantes.Estados.ESTADO_SP || estadoDestino_ == Constantes.Estados.ESTADO_MG ||
                estadoDestino_ == Constantes.Estados.ESTADO_RJ || estadoDestino_ == Constantes.Estados.ESTADO_ES;       
        }

        /// <summary>
        /// Metodo responsavel pelo preenchimento do item da  nota fiscal
        /// </summary>
        /// <param name="notaFiscal">Nota Fiscal</param>
        /// <param name="itemPedido">Item de peido</param>
        /// <returns>Item da Nota Preenchida</returns>
        private NotaFiscalItem GerarNotaFiscalItem(NotaFiscal notaFiscal, PedidoItem itemPedido)
        {
            NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

            if (PreencherDescontoSudeste(notaFiscal.EstadoDestino))
            {
                notaFiscalItem.Desconto = itemPedido.ValorItemPedido * 0.1;
                itemPedido.ValorItemPedido = itemPedido.ValorItemPedido - notaFiscalItem.Desconto;
            }

            notaFiscalItem.Cfop = BuscarCFOP(notaFiscal.EstadoOrigem, notaFiscal.EstadoDestino);
            SetaraICMS(notaFiscal.EstadoDestino, notaFiscal.EstadoOrigem, ref notaFiscalItem, itemPedido);            

            notaFiscalItem.Ipi.BaseIPI = itemPedido.ValorItemPedido;

            if (itemPedido.Brinde)
            {
                notaFiscalItem.Icms.TipoIcms = Constantes.ICMS.TIPO_ICMS_BRINDE;
                notaFiscalItem.Icms.AliquotaIcms = Constantes.ICMS.ALIQUOTA_ICMS_BRINDE;
                notaFiscalItem.Icms.ValorIcms = notaFiscalItem.Icms.BaseIcms * notaFiscalItem.Icms.AliquotaIcms;
                notaFiscalItem.Ipi.AliquotaIPI = Constantes.IPI.ALIQUOTA_IPI_BRINDE;
            }
            else
            {
                notaFiscalItem.Ipi.AliquotaIPI = Constantes.IPI.ALIQUOTA_IPI_COMPRA;
            }

            notaFiscalItem.Ipi.ValorIPI = notaFiscalItem.Ipi.BaseIPI * notaFiscalItem.Ipi.AliquotaIPI;

            notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
            notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

            return notaFiscalItem;
        }

        /// <summary>
        /// Metodo responsavel por buscar o código do CFOP a partir dos estados de origem e de destino do pedido
        /// </summary>
        /// <param name="estadoOrigem_">Estado de Origem</param>
        /// <param name="estadoDestino_">Estado de Pedido</param>
        /// <returns>Codigo do CFOP encontrado</returns>
        private string BuscarCFOP(string estadoOrigem_, string estadoDestino_)
        {
            string cfop = string.Empty;

            if (estadoOrigem_ == Constantes.Estados.ESTADO_SP || estadoOrigem_ == Constantes.Estados.ESTADO_MG)
            {
                cfop = BuscarCFOPEstado(estadoDestino_);
            }            

            return cfop;
        }

        /// <summary>
        /// Metodo responsavel por buscar o código do CFOP a partir dos estados de destino do pedido
        /// </summary>        
        /// <param name="estadoDestino_">Estado de Pedido</param>
        /// <returns>Codigo do CFOP encontrado</returns>
        private string BuscarCFOPEstado(string estadoDestino_)
        {
            string cfop = string.Empty;            

            switch (estadoDestino_)
            {
                case Constantes.Estados.ESTADO_RJ:
                    cfop = Constantes.CFOP.VALOR_CFOP_RJ;
                    break;

                case Constantes.Estados.ESTADO_PE:
                    cfop = Constantes.CFOP.VALOR_CFOP_PE;
                    break;

                case Constantes.Estados.ESTADO_MG:
                    cfop = Constantes.CFOP.VALOR_CFOP_MG;
                    break;

                case Constantes.Estados.ESTADO_PB:
                    cfop = Constantes.CFOP.VALOR_CFOP_PB;
                    break;

                case Constantes.Estados.ESTADO_PR:
                    cfop = Constantes.CFOP.VALOR_CFOP_PR;
                    break;

                case Constantes.Estados.ESTADO_PI:
                    cfop = Constantes.CFOP.VALOR_CFOP_PI;
                    break;

                case Constantes.Estados.ESTADO_RO:
                    cfop = Constantes.CFOP.VALOR_CFOP_RO;
                    break;

                case Constantes.Estados.ESTADO_SE:
                    cfop = Constantes.CFOP.VALOR_CFOP_SE;
                    break;

                case Constantes.Estados.ESTADO_TO:
                    cfop = Constantes.CFOP.VALOR_CFOP_TO;
                    break;

                case Constantes.Estados.ESTADO_PA:
                    cfop = Constantes.CFOP.VALOR_CFOP_PA;
                    break;                               
            }

            return cfop;
        }        

        /// <summary>
        /// Metodo responsavel por preencher o valor dos dados  referentes a ICMS
        /// </summary>
        /// <param name="estadoDestino_">Estado de Destino</param>
        /// <param name="estadoOrigem_">Estado de Origem</param>
        /// <param name="notaFiscalItem_">Item de Nota Fiscal</param>
        /// <param name="itemPedido_">Item de pedido</param>
        private void SetaraICMS(string estadoDestino_, string estadoOrigem_, ref NotaFiscalItem notaFiscalItem_, PedidoItem itemPedido_)
        {
            if (estadoDestino_ == estadoOrigem_)
            {
                notaFiscalItem_.Icms.TipoIcms = Constantes.ICMS.TIPO_ICMS_ESTADO_ORIGEM_DESTINO_IGUAIS;
                notaFiscalItem_.Icms.AliquotaIcms = Constantes.ICMS.ALIQUOTA_ICMS_ESTADO_ORIGEM_DESTINO_IGUAIS;
            }
            else
            {
                notaFiscalItem_.Icms.TipoIcms = Constantes.ICMS.TIPO_ICMS_ESTADO_ORIGEM_DESTINO_DIFERENTES;
                notaFiscalItem_.Icms.AliquotaIcms = Constantes.ICMS.ALIQUOTA_ICMS_ESTADO_ORIGEM_DESTINO_DIFERENTES;
            }

            if (notaFiscalItem_.Cfop == Constantes.CFOP.VALOR_CFOP_SE)
            {
                notaFiscalItem_.Icms.BaseIcms = itemPedido_.ValorItemPedido * 0.90; //redução de base
            }
            else
            {
                notaFiscalItem_.Icms.BaseIcms = itemPedido_.ValorItemPedido;
            }

            notaFiscalItem_.Icms.ValorIcms = notaFiscalItem_.Icms.BaseIcms * notaFiscalItem_.Icms.AliquotaIcms;
        }

        #endregion
    }
}

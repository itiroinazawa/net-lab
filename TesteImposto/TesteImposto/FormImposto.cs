using Imposto.Core.Service;
using Imposto.Domain;
using Imposto.Helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();              

        /// <summary>
        /// Construtor do FormImposto
        /// </summary>
        public FormImposto()
        {
            InitializeComponent();
            dataGridViewPedidos.AutoGenerateColumns = true;                       
            dataGridViewPedidos.DataSource = GetTablePedidos();
            ResizeColumns();
        }

        #region Metodos Privados

        /// <summary>
        /// Metodo responsavel por redefinir o tamanho das colunas do DataGrid
        /// </summary>
        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }   
        }

        /// <summary>
        /// Metodo responsavel por criar o datatable a ser utilizado no datagrid
        /// </summary>
        /// <returns>DataTable criado</returns>
        private object GetTablePedidos()
        {            
            DataTable table = new DataTable(Constantes.Tabelas.TABELA_PEDIDOS);
            table.Columns.Add(new DataColumn(Constantes.Tabelas.COLUNA_NOME_PRODUTO, typeof(string)));
            table.Columns.Add(new DataColumn(Constantes.Tabelas.COLUNA_CODIGO_PRODUTO, typeof(string)));
            table.Columns.Add(new DataColumn(Constantes.Tabelas.COLUNA_VALOR, typeof(decimal)));
            table.Columns.Add(new DataColumn(Constantes.Tabelas.COLUNA_BRINDE, typeof(bool)));
                     
            return table;
        }

        /// <summary>
        /// Metodo responsavel por limpar os dados da tela 
        /// </summary>
        private void LimparTela()
        {
            txtEstadoOrigem.Clear();
            txtEstadoDestino.Clear();
            textBoxNomeCliente.Clear();

            dataGridViewPedidos.DataSource = GetTablePedidos();
            dataGridViewPedidos.Update();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Metodo executado ao clicar no botão buttonGerarNotaFiscal
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            NotaFiscalService service = new NotaFiscalService();
            EstadoService estadoService = new EstadoService();

            if (!estadoService.ValidarEstado(txtEstadoOrigem.Text))
            {
                MessageBox.Show(string.Format(Constantes.Mensagens.ESTADO_INVALIDO, Constantes.Mensagens.ORIGEM));
                return;
            }

            if (!estadoService.ValidarEstado(txtEstadoDestino.Text))
            {
                MessageBox.Show(string.Format(Constantes.Mensagens.ESTADO_INVALIDO, Constantes.Mensagens.DESTINO));
                return;
            }

            pedido.EstadoOrigem = txtEstadoOrigem.Text;
            pedido.EstadoDestino = txtEstadoDestino.Text;
            pedido.NomeCliente = textBoxNomeCliente.Text;

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            foreach (DataRow row in table.Rows)
            {
                PedidoItem item = new PedidoItem();

                item.Brinde = row[Constantes.Tabelas.COLUNA_BRINDE] != DBNull.Value ? Convert.ToBoolean(row[Constantes.Tabelas.COLUNA_BRINDE]) : false;
                item.CodigoProduto = row[Constantes.Tabelas.COLUNA_CODIGO_PRODUTO].ToString();
                item.NomeProduto = row[Constantes.Tabelas.COLUNA_NOME_PRODUTO].ToString();
                item.ValorItemPedido = Convert.ToDouble(row[Constantes.Tabelas.COLUNA_VALOR].ToString());

                pedido.ItensDoPedido.Add(item);
            }

            service.GerarNotaFiscal(pedido);
            LimparTela();
            MessageBox.Show(Constantes.Mensagens.OPERACAO_SUCESSO);
        }

        #endregion
    }
}

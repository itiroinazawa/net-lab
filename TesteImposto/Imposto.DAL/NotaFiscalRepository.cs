using Imposto.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.DAL
{
    public class NotaFiscalRepository
    {        
        /// <summary>
        /// Metodo responsavel por atualizar as informações da Nota Fiscal
        /// </summary>
        /// <param name="iD_">ID da Nota Fiscal</param>
        /// <param name="numeroNotaFiscal_">Numero da Nota Fiscal</param>
        /// <param name="serie_">Serie da Nota Fiscal</param>
        /// <param name="nomeCliente_">Nome do Cliente</param>
        /// <param name="estadoDestino_">Estado de Destino</param>
        /// <param name="estadoOrigem_">Estado de Origem</param>
        /// <returns>ID da Nota Fiscal</returns>
        public int CreateUpdateNotaFiscal(int iD_, int numeroNotaFiscal_, int serie_, string nomeCliente_, string estadoDestino_, string estadoOrigem_)
        {
            using (SqlConnection conn = Connection.Instance.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(Constantes.Procedures.PROCEDURE_NOTA_FISCAL, conn);

                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ID, iD_).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_NUMERO_NOTA_FISCAL, numeroNotaFiscal_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_SERIE, serie_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_NOME_CLIENTE, nomeCliente_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ESTADO_DESTINO, estadoDestino_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ESTADO_ORIGEM, estadoOrigem_);

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    object obj = cmd.ExecuteNonQuery();                
                    iD_ = Convert.ToInt32(cmd.Parameters[Constantes.Parametros.PARAMETRO_ID].Value);
                }
                catch
                {
                    iD_ = 0;
                }
                finally
                {
                    conn.Close();
                }
            }

            return iD_;
        }
    }
}

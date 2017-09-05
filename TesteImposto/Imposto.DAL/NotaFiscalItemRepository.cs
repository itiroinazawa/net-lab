using Imposto.Domain;
using Imposto.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.DAL
{
    public class NotaFiscalItemRepository
    {                  
        /// <summary>
        /// Metodo responsavel por atualizar as informacoes do item da nota fiscal na base de dados
        /// </summary>
        /// <param name="iD">ID do item</param>
        /// <param name="iDNotaFiscal">ID da Nota Fiscal</param>
        /// <param name="cFOP_">CFOP</param>
        /// <param name="tipoICMS_">Tipo de ICMS</param>
        /// <param name="baseICMS_">Valor Base de ICMS</param>
        /// <param name="aliquotaICMS_">Aliquota do ICMS</param>
        /// <param name="valorICMS_">Valor do ICMS</param>
        /// <param name="nomeProduto_">Nome do Produto</param>
        /// <param name="codigoProduto_">Codigo do Produto</param>
        /// <param name="baseIPI_">Valor Base de IPI</param>
        /// <param name="aliquotaIPI_">Aliquota de IPI</param>
        /// <param name="valorIPI_">Valor de IPI</param>
        /// <returns>Status de atualização do registro</returns>
        public bool CreateUpdateNotaFiscalItem(int iD, int iDNotaFiscal, string cFOP_, string tipoICMS_, double baseICMS_,
            double aliquotaICMS_, double valorICMS_, string nomeProduto_, string codigoProduto_,
            double baseIPI_, double aliquotaIPI_, double valorIPI_)
        {
            bool resultado = false;

            using (SqlConnection conn = Connection.Instance.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(Constantes.Procedures.PROCEDURE_NOTA_FISCAL_ITEM, conn);

                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ID, iD).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ID_NOTA_FISCAL, iDNotaFiscal);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_CFOP, cFOP_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_TIPO_ICMS, tipoICMS_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_BASE_ICMS, baseICMS_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_ALIQUOTA_ICMS, aliquotaICMS_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_VALOR_ICMS, valorICMS_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_NOME_PRODUTO, nomeProduto_);
                cmd.Parameters.AddWithValue(Constantes.Parametros.PARAMETRO_CODIGO_PRODUTO, codigoProduto_);

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    object obj = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Metodo responsavel por retornar as informações de IPI e ICMS agrupadas por CFOP
        /// </summary>
        /// <returns>Lista de Agrupamento de CFOP</returns>
        public List<AgrupamentoCFOP> ConsultarAgrupamentoCFOP()
        {
            List<AgrupamentoCFOP> lista = new List<AgrupamentoCFOP>();

            using (SqlConnection conn = Connection.Instance.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(Constantes.Procedures.PROCEDURE_CFOP_AGRUPADO, conn);

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AgrupamentoCFOP item = new AgrupamentoCFOP
                            {
                                CFOP = reader[Constantes.ColunasSQL.COLUNA_CFOP].ToString(),

                                BaseICMSTotal = Convert.ToDouble(reader[Constantes.ColunasSQL.COLUNA_BASE_ICMS].ToString()),
                                ValorICMSTotal = Convert.ToDouble(reader[Constantes.ColunasSQL.COLUNA_VALOR_ICMS].ToString()),

                                ValorIPITotal = Convert.ToDouble(reader[Constantes.ColunasSQL.COLUNA_BASE_IPI].ToString()),
                                BaseIPITotal = Convert.ToDouble(reader[Constantes.ColunasSQL.COLUNA_VALOR_IPI].ToString())
                            };

                            lista.Add(item);
                        }
                    }
                }
                catch
                {
                    lista = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return lista;
        }
    }
}

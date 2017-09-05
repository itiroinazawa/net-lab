namespace Imposto.Helpers
{
    /// <summary>
    /// Classe responsavel por agrupar todos os valores que estariam Hard-Coded e 
    /// Utilizar constantes para reutilizar valores que se repetem, assim como agrupar melhor as informações    
    /// </summary>
    public static class Constantes
    {
        public static class IPI
        {
            public const double ALIQUOTA_IPI_BRINDE = 0;
            public const double ALIQUOTA_IPI_COMPRA = 0.1;
        }

        public static class ICMS
        {
            public const string TIPO_ICMS_BRINDE = "60";
            public const string TIPO_ICMS_ESTADO_ORIGEM_DESTINO_IGUAIS = "60";
            public const string TIPO_ICMS_ESTADO_ORIGEM_DESTINO_DIFERENTES = "10";

            public const double ALIQUOTA_ICMS_BRINDE = 0.18;
            public const double ALIQUOTA_ICMS_ESTADO_ORIGEM_DESTINO_IGUAIS = 0.18;
            public const double ALIQUOTA_ICMS_ESTADO_ORIGEM_DESTINO_DIFERENTES = 0.17;
        }

        public static class Estados
        {
            public const string ESTADO_RJ = "RJ";
            public const string ESTADO_PE = "PE";
            public const string ESTADO_MG = "MG";
            public const string ESTADO_PB = "PB";
            public const string ESTADO_PR = "PR";
            public const string ESTADO_PI = "PI";
            public const string ESTADO_RO = "RO";
            public const string ESTADO_SE = "SE";
            public const string ESTADO_TO = "TO";
            public const string ESTADO_PA = "PA";
            public const string ESTADO_SP = "SP";
            public const string ESTADO_ES = "ES";
        }

        public static class CFOP
        {
            public const string VALOR_CFOP_RJ = "6.000";
            public const string VALOR_CFOP_PE = "6.001";
            public const string VALOR_CFOP_MG = "6.002";
            public const string VALOR_CFOP_PB = "6.003";
            public const string VALOR_CFOP_PR = "6.004";
            public const string VALOR_CFOP_PI = "6.005";
            public const string VALOR_CFOP_RO = "6.006";
            public const string VALOR_CFOP_SE = "6.009";
            public const string VALOR_CFOP_TO = "6.008";
            public const string VALOR_CFOP_PA = "6.010";
        }

        public static class Tabelas
        {
            public const string TABELA_PEDIDOS = "pedidos";
            public const string COLUNA_NOME_PRODUTO = "Nome do produto";
            public const string COLUNA_CODIGO_PRODUTO = "Codigo do produto";
            public const string COLUNA_VALOR = "Valor";
            public const string COLUNA_BRINDE = "Brinde";
        }

        public static class Mensagens
        {
            public const string OPERACAO_SUCESSO = "Operação efetuada com sucesso";
            public const string ESTADO_INVALIDO = "Estado de {0} inválido";
            public const string DESTINO = "Destino";
            public const string ORIGEM = "Origem";
        }

        public static class Random
        {
            public const string NOME_NOTA_FISCAL = "NotaFiscal{0}{1}.xml";
            public const string CONNECTION_STRING = "ConnectionString";
        }

        public static class Procedures
        {
            public const string PROCEDURE_NOTA_FISCAL_ITEM = "P_NOTA_FISCAL_ITEM";
            public const string PROCEDURE_CFOP_AGRUPADO = "P_CFOP_AGRUPADO";
            public const string PROCEDURE_NOTA_FISCAL = "P_NOTA_FISCAL";
        }

        public static class ColunasSQL
        {
            public const string COLUNA_CFOP = "CFOP";
            public const string COLUNA_BASE_ICMS = "BASEICMS";
            public const string COLUNA_VALOR_ICMS = "VALORICMS";
            public const string COLUNA_BASE_IPI = "BASEIPI";
            public const string COLUNA_VALOR_IPI = "VALORIPI";
        }

        public static class Parametros
        {
            public const string PARAMETRO_ID = "@pId";
            public const string PARAMETRO_ID_NOTA_FISCAL = "@pIdNotaFiscal";
            public const string PARAMETRO_CFOP = "@pCfop";
            public const string PARAMETRO_TIPO_ICMS = "@pTipoIcms";
            public const string PARAMETRO_BASE_ICMS = "@pBaseIcms";
            public const string PARAMETRO_ALIQUOTA_ICMS = "@pAliquotaIcms";
            public const string PARAMETRO_VALOR_ICMS = "@pValorIcms";
            public const string PARAMETRO_NOME_PRODUTO = "@pNomeProduto";
            public const string PARAMETRO_CODIGO_PRODUTO = "@pCodigoProduto";            
            public const string PARAMETRO_NUMERO_NOTA_FISCAL = "@pNumeroNotaFiscal";
            public const string PARAMETRO_SERIE = "@pSerie";
            public const string PARAMETRO_NOME_CLIENTE = "@pNomeCliente";
            public const string PARAMETRO_ESTADO_DESTINO = "@pEstadoDestino";
            public const string PARAMETRO_ESTADO_ORIGEM = "@pEstadoOrigem";
        }
    }
}

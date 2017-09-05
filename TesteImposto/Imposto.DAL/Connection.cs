using Imposto.Helpers;
using System.Configuration;
using System.Data.SqlClient;

namespace Imposto.DAL
{
    public sealed class Connection
    {        
        static SqlConnection con;
        static Connection instance = null;

        /// <summary>
        /// Singleton para conexão de banco de dados
        /// </summary>
        public static Connection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connection();
                }
                return instance;
            }
        }

        /// <summary>
        /// Metodo responsavel por retornar a conexao de banco de dados
        /// </summary>
        /// <returns>SQL Connection</returns>
        public SqlConnection GetConnection()
        {
            if (con == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[Constantes.Random.CONNECTION_STRING].ConnectionString;
                con = new SqlConnection(connectionString);
            }

            if (con.State != System.Data.ConnectionState.Open)
            {
                OpenConnection();
            }

            return con;
        }

        /// <summary>
        /// Metodo responsavel por abrir a conexão de banco de dados
        /// </summary>
        private void OpenConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[Constantes.Random.CONNECTION_STRING].ConnectionString;
            con.ConnectionString = connectionString;
            con.Open();
        }

        /// <summary>
        /// Metodo responsavel por fechar a conexão de banco de dados
        /// </summary>
        public void CloseConnection()
        {
            con.Dispose();
            con.Close();            
        }
    }
}

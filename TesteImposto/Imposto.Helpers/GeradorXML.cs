using Imposto.Domain;
using System.IO;
using System.Xml.Serialization;

namespace Imposto.Helpers
{
    public class GeradorXML
    {
        /// <summary>
        /// Metodo responsavel por gerar o XML da Nota Fiscal
        /// </summary>
        /// <param name="path_">Caminho para a gravacao da Nota Fiscal</param>
        /// <param name="notaFiscal_">Nota Fiscal</param>
        /// <returns>Status de geração do XML</returns>
        public bool GerarXML(string path_, NotaFiscal notaFiscal_)
        {
            bool resultado = false;

            XmlSerializer serializer = new XmlSerializer(typeof(NotaFiscal));

            if (!string.IsNullOrEmpty(path_) && notaFiscal_ != null)
            {
                try
                {
                    string nome = string.Format(Constantes.Random.NOME_NOTA_FISCAL, notaFiscal_.NumeroNotaFiscal, notaFiscal_.Serie);
                    path_ = string.Concat(path_, nome);

                    using (TextWriter writer = new StreamWriter(path_))
                    {
                        serializer.Serialize(writer, notaFiscal_);
                        resultado = true;
                    }
                }
                catch
                {
                    resultado = false;
                }
            }

            return resultado;
        }
    }
}

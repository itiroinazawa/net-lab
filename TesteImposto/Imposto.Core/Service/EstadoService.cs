using Imposto.Helpers;

namespace Imposto.Core.Service
{
    public class EstadoService
    {
        /// <summary>
        /// Metodo responsavel por validar se o estado inserido é valido
        /// </summary>
        /// <param name="estado_">Estado a ser verificado</param>
        /// <returns>Estado validado</returns>
        public bool ValidarEstado(string estado_)
        {
            switch (estado_)
            {
                case Constantes.Estados.ESTADO_RJ:
                case Constantes.Estados.ESTADO_PE:
                case Constantes.Estados.ESTADO_MG:
                case Constantes.Estados.ESTADO_PB:
                case Constantes.Estados.ESTADO_PR:
                case Constantes.Estados.ESTADO_PI:
                case Constantes.Estados.ESTADO_RO:
                case Constantes.Estados.ESTADO_SE:
                case Constantes.Estados.ESTADO_TO:
                case Constantes.Estados.ESTADO_PA:
                case Constantes.Estados.ESTADO_SP:
                    return true;
                default:
                    return false;
            }
        }
    }
}

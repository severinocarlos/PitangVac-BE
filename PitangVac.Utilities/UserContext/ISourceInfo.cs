using System.Collections;
using System.Net;

namespace PitangVac.Utilities.UserContext
{
    public interface ISourceInfo
    {
        /// <summary>
        /// Contém HEADERS da requisição.
        /// </summary>
        Hashtable Data { get; set; }

        /// <summary>
        /// Origem da requisição.
        /// </summary>
        IPAddress IP { get; set; }
    }
}

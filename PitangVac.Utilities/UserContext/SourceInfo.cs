using System.Collections;
using System.Net;

namespace PitangVac.Utilities.UserContext
{
    public class SourceInfo : ISourceInfo
    {
        /// <summary>
        /// Contém HEADERS da requisição.
        /// </summary>
        public Hashtable Data { get; set; }

        /// <summary>
        /// Origem da requisição.
        /// </summary>
        public IPAddress IP { get; set; }
    }
}

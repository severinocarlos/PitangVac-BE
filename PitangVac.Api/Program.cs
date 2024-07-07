using log4net.Config;
using log4net;
using Microsoft.AspNetCore;
using System.Reflection;

namespace PitangVac.Api
{
    public static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetCallingAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                _log.Info("Iniciando a API");
                var webHost = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

                webHost.Build().Run();
            }
            catch (Exception ex)
            {
                _log.Fatal("Erro fatal", ex);
                throw;
            }
        }
    }
}
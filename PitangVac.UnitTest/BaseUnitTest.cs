using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PitangVac.Repository;

namespace PitangVac.UnitTest
{
    public class BaseUnitTest
    {
        private readonly IServiceCollection ServiceCollection = new ServiceCollection();
        protected DatabaseContext _DbContext;

        protected void Registrar<I, T>() where I : class where T : class, I
          => ServiceCollection.AddSingleton<I, T>();

        protected I ObterServico<I>() where I : class
          => ServiceCollection.BuildServiceProvider().GetService<I>();

        protected void RegistrarObjeto<Tp, T>(Tp type, T objeto) where Tp : Type where T : class
           => ServiceCollection.AddSingleton(type, objeto);

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            ConfigureInMemoryDataBase();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            _DbContext.Dispose();
        }

        private void ConfigureInMemoryDataBase()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                              .UseInMemoryDatabase("InMemoryDatabase")
            .Options;

            _DbContext = new DatabaseContext(options);

            if (_DbContext.Database.IsInMemory())
                _DbContext.Database.EnsureDeleted();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Senparc.Scf.Core.Tests
{
    [TestClass]
    public class TestBase
    {
        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();

        public TestBase() { }
    }
}

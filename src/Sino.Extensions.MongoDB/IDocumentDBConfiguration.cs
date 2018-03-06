using Sino.Dependency;

namespace Sino.Extensions.MongoDB
{
    public interface IDocumentDBConfiguration : ISingletonDependency
    {
        string Host { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string DataBase { get; set; }

        bool NoTotal { get; set; }
    }
}

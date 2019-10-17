using Sino.Extensions.MongoDB;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DocumentDBServiceCollectionExtensions
    {
        /// <summary>
        /// 使用DocumentDB
        /// </summary>
        /// <param name="endpointUri">数据库地址</param>
        /// <param name="primaryKey">访问密钥</param>
        /// <param name="database">数据库名</param>
        /// <param name="noTotal">列表是否需要Total（True为不需要Total这样列表会加快10ms+，默认为false）</param>
        public static IServiceCollection AddDocumentDB(this IServiceCollection services, string host, string userName, string password, string dataBase, bool noTotal = false, bool ssl = true)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));
            if (string.IsNullOrEmpty(dataBase))
                throw new ArgumentNullException(nameof(dataBase));

            services.AddSingleton<IDocumentDBConfiguration>(new DocumentDBConfiguration
            {
                Host = host,
                UserName = userName,
                Password = password,
                DataBase = dataBase,
                NoTotal = noTotal,
                SSL = ssl
            });
            return services;
        }
    }
}

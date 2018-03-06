using System;

namespace Sino.Extensions.MongoDB
{
    public class DocumentDBConfiguration : IDocumentDBConfiguration
    {
        /// <summary>
        /// 数据库地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// 列表是否需要Total（True为不需要Total这样列表会加快10ms+，默认为false）
        /// </summary>
        public bool NoTotal { get;set;}
    }
}

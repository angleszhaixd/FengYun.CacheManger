using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CacheManager.Logging;
using static CacheManager.Core.Utility.Guard;

namespace CacheManager.Core
{
    public static class NLoggingBuilderExtensions
    {
        /// <summary>
        /// Enables logging for the cache manager instance.
        /// This will add an <see cref="Logging.ILoggerFactory"/> using the <c>NLog</c> framework.
        /// </summary>
        /// <param name="part">The builder part.</param>
        /// <returns>The builder.</returns>
        public static ConfigurationBuilderCachePart WithNLogLogging(this ConfigurationBuilderCachePart part)
        {
            NotNull(part, nameof(part));
            return part.WithLogging(typeof(NLoggerFactoryAdapter));
        }
    }
}

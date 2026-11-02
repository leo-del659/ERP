using System;

namespace Demo.Infrastructure.Tools
{
    public static class Config
    {
        public static string NewSequentialGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Shared.Constants
{
    public class ApiHeaderKeysConst
    {
        public const string RefreshToken = "refreshToken";
        public const string ASPNETCORE_ENVIRONMENT = nameof(ASPNETCORE_ENVIRONMENT);
        public const string Production = nameof(Production);
        public const string XForwardedFor = "X-Forwarded-For";
        public const string CertSerialNumber = "certSerialNumber";
        public const string CertSubject = "certSubject";
    }
}

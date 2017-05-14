using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terministrator.Terministrator.BLL
{
    public static class Link
    {
        public static Uri GetUri(string urlString)
        {
            return Uri.TryCreate(urlString, UriKind.Absolute, out Uri uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                 || uri.Scheme == Uri.UriSchemeHttps
                 || uri.Scheme == Uri.UriSchemeFtp) ? uri : null;
        }
    }
}

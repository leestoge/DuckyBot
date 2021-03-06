﻿using System;
using System.Net;

namespace DuckyBot.Core.Utilities
{
    public static class ApiHelper
    {
        public static string GetRedirectUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            var newUrl = url;
            {
                HttpWebResponse resp = null;
                try
                {
                    var req = (HttpWebRequest) WebRequest.Create(url);
                    req.Method = "HEAD";
                    req.AllowAutoRedirect = false;
                    resp = (HttpWebResponse) req.GetResponse();
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return newUrl;
                        case HttpStatusCode.Redirect:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.RedirectKeepVerb:
                        case HttpStatusCode.RedirectMethod:

                            newUrl = resp.Headers["Location"];
                            if (newUrl == null)
                            {
                                return url;
                            }

                            if (newUrl.IndexOf("://", StringComparison.Ordinal) == -1)
                            {
                                // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                                var u = new Uri(new Uri(url), newUrl);
                                newUrl = u.ToString();
                            }

                            break;
                        default:throw new InvalidOperationException();
                    }

                    return newUrl;
                }
                catch (WebException)
                {
                    // Return the last known good URL
                    return newUrl;
                }
                #pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (resp != null)
                    {
                        resp.Close();
                    }
                }
            }
        }
    }
}
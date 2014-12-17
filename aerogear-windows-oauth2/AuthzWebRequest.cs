﻿using System;
using System.Net;
using System.Threading.Tasks;

namespace AeroGear.OAuth2
{
    public class AuthzWebRequest : WebRequest
    {
        private WebRequest webRequest;

        public AuthzWebRequest(WebRequest request)
        {
            this.webRequest = request;
        }

        public AuthzWebRequest(WebRequest request, AuthzModule authzModule) : this(request)
        {
            this.authzModule = authzModule;
            authzModule.RequestAccess();
        }

        private AuthzModule authzModule;

        public new static WebRequest Create(string requestUriString)
        {
            var request = WebRequest.Create(requestUriString);
            return new AuthzWebRequest(request);
        }

        public static WebRequest Create(string requestUriString, AuthzModule authzModule)
        {
            var request = WebRequest.Create(requestUriString);
            return new AuthzWebRequest(request, authzModule);
        }

        public new static WebRequest Create(Uri requestUri)
        {
            var webRequest = WebRequest.Create(requestUri);
            return new AuthzWebRequest(webRequest);
        }

        public static WebRequest Create(Uri requestUri, AuthzModule authzModule)
        {
            var webRequest = WebRequest.Create(requestUri);
            return new AuthzWebRequest(webRequest, authzModule);
        }

        public override void Abort()
        {
            webRequest.Abort();
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            if (authzModule != null)
            {
                var field = authzModule.AuthorizationFields();
                webRequest.Headers[field.Item1] = field.Item2;
            }
            return webRequest.BeginGetRequestStream(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return webRequest.BeginGetResponse(callback, state);
        }

        public override string ContentType
        {
            get
            {
                return webRequest.ContentType;
            }
            set
            {
                webRequest.ContentType = value;
            }
        }

        public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return webRequest.EndGetRequestStream(asyncResult);
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return webRequest.EndGetResponse(asyncResult);
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                return webRequest.Headers;
            }
            set
            {
                webRequest.Headers = value;
            }
        }

        public override string Method
        {
            get
            {
                return webRequest.Method;
            }
            set
            {
                webRequest.Method = value;
            }
        }

        public override Uri RequestUri
        {
            get { return webRequest.RequestUri; }
        }
    }
}

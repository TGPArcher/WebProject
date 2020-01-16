using CrudToDo.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CrudToDo.Authentication
{
    public class BasicAuthMiddleware
    {
        private UserService userService;
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, UserService userService)
        {
            this.userService = userService;

            if(context.Request.Path == "/api/auth/register")
            {
                await this._next.Invoke(context);
            }
            else
            {
                if (CheckIsValidRequest(context, out string username))
                {
                    var identity = new GenericIdentity(username);
                    var principle = new GenericPrincipal(identity, null);
                    context.User = principle;
                    await this._next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
        }

        private bool CheckIsValidRequest(HttpContext context, out string username)
        {
            var basicAuthHeader = GetBasicAuthenticationHeaderValue(context);
            username = basicAuthHeader.UserName;
            return basicAuthHeader.IsValidBasicAuthenticationHeaderValue &&
                   IsUserValid(basicAuthHeader.UserName, basicAuthHeader.Password);
        }

        private BasicAuthenticationHeaderValue GetBasicAuthenticationHeaderValue(HttpContext context)
        {
            var basicAuthenticationHeader = context.Request.Headers["Authorization"]
                .FirstOrDefault(header => header.StartsWith("Basic", StringComparison.OrdinalIgnoreCase));
            var decodedHeader = new BasicAuthenticationHeaderValue(basicAuthenticationHeader);
            return decodedHeader;
        }

        private bool IsUserValid(string username, string password)
        {
            var user = userService.GetUserByUsername(username);
            return user.Password == password;
        }
    }

    public class BasicAuthenticationHeaderValue
    {
        private readonly string _authenticationHeaderValue;
        private string[] _splitDecodedCredentials;
        public bool IsValidBasicAuthenticationHeaderValue { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public BasicAuthenticationHeaderValue(string authenticationHeaderValue)
        {
            if (!string.IsNullOrWhiteSpace(authenticationHeaderValue))
            {
                this._authenticationHeaderValue = authenticationHeaderValue;
                if (TryDecodeHeaderValue())
                {
                    ReadAuthenticationHeaderValue();
                }
            }
        }

        private bool TryDecodeHeaderValue()
        {
            const int headerSchemeLength = 6;
            if (this._authenticationHeaderValue.Length <= headerSchemeLength)
            {
                return false;
            }
            var encodedCredentials = this._authenticationHeaderValue.Substring(headerSchemeLength);
            try
            {
                var decodedCredentials = Convert.FromBase64String(encodedCredentials);
                this._splitDecodedCredentials = System.Text.Encoding.ASCII.GetString(decodedCredentials).Split(':');
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void ReadAuthenticationHeaderValue()
        {
            IsValidBasicAuthenticationHeaderValue = this._splitDecodedCredentials.Length == 2
                                                   && !string.IsNullOrWhiteSpace(this._splitDecodedCredentials[0])
                                                   && !string.IsNullOrWhiteSpace(this._splitDecodedCredentials[1]);
            if (IsValidBasicAuthenticationHeaderValue)
            {
                UserName = this._splitDecodedCredentials[0];
                Password = this._splitDecodedCredentials[1];
            }
        }
    }
}
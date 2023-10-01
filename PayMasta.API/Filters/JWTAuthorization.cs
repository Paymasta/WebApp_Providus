using Microsoft.IdentityModel.Tokens;
using PayMasta.Service.Account;
using PayMasta.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PayMasta.API.Filters
{

    /// <summary>
    /// JWTAuthorization
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class JWTAuthorization : AuthorizationFilterAttribute
    {
        private List<EnumUserType> _roles;
        private RoleSettings _roleSetting;
        private IAccountService _accountService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="roles"></param>
        public JWTAuthorization(RoleSettings setting, params EnumUserType[] roles)
        {
            if (_roles == null)
                _roles = new List<EnumUserType>(0);

            foreach (var role in roles)
            {
                _roles.Add(role);
            }
            _roleSetting = setting;
        }

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var langId = "2";// actionContext.Request.Headers.GetValues("languagePreference").FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(langId))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langId == "2" ? "ar" : "en");
                }

                if (_roleSetting != RoleSettings.NoAction)
                {
                    var rawToken = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                    var deviceId = actionContext.Request.Headers.GetValues("DeviceId").FirstOrDefault();

                    var token = rawToken.ToString().Substring("Bearer ".Length);
                    string secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

                    var key = Encoding.UTF8.GetBytes(secret);// Encoding.ASCII.GetBytes(secret);
                    var handler = new JwtSecurityTokenHandler();
                    var tokenSecure = handler.ReadToken(token) as SecurityToken;



                    //var stream = rawToken;
                    //var handler = new JwtSecurityTokenHandler();
                    //var tokenSecure = handler.ReadToken(stream);
                    //var tokenS = tokenSecure as JwtSecurityToken;

                    var validations = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    var time = tokenSecure.ValidTo;
                    var claims = handler.ValidateToken(token, validations, out tokenSecure);

                    var rawRoleHeader = claims.Claims.FirstOrDefault(claim => claim.Type == "typ").Value;

                    bool isInvalidUser = false;
                    bool isSession = false;
                    var userGuid = claims.Claims.FirstOrDefault(claim => claim.Type == "jti").Value;
                    if (userGuid != null)
                    {
                        _accountService = new AccountService();
                        isInvalidUser = _accountService.CheckInvalidUser(Guid.Parse(userGuid));
                        isSession = _accountService.IsSessionValid(deviceId);
                    }

                    bool isInvalidRole = IsInvalidRole(rawRoleHeader);
                    if (time.Date < DateTime.UtcNow || isInvalidRole || isInvalidUser || !isSession)
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                    }
                    else
                    {
                        //var claims = handler.ValidateToken(token, validations, out tokenSecure);

                        //string jti = Guid.Empty.ToString();
                        //jti = claims.Claims.First(claim => claim.Type == "jti").Value;
                        //FillUserGuidFromToken(actionContext, jti);
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }

        //private void FillUserGuidFromToken(HttpActionContext actionContext, string guid)
        //{
        //    try
        //    {
        //        string jsonBodyStream;
        //        using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
        //        {
        //            stream.BaseStream.Position = 0;
        //            jsonBodyStream = stream.ReadToEnd();
        //        }
        //        var jsonBody = new StringBuilder();

        //        using (var reader = new StreamReader(jsonBodyStream))
        //        {
        //            jsonBody.Append(reader.ReadToEnd());
        //        }

        //        dynamic jsonStream = JsonConvert.DeserializeObject(jsonBody.ToString());

        //        StringBuilder updatedStream = new StringBuilder();

        //        if (jsonStream != null)
        //        {
        //            jsonStream.userguid = guid;
        //            updatedStream.Append(JsonConvert.SerializeObject(jsonStream));
        //        }
        //        else
        //        {
        //            jsonStream = new
        //            {
        //                userguid = guid
        //            };
        //            updatedStream.Append("{@updatedData}");
        //            updatedStream.Replace("@updatedData", JsonConvert.SerializeObject(jsonStream));
        //        }

        //        var newBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(updatedStream.ToString()));

        //        context.HttpContext.Request.Body = newBodyStream;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private bool IsInvalidRole(string roleHeader)
        {
            bool response = false;

            try
            {
                bool roleExistsInList = false;

                int currentUserRole = -1;

                if (int.TryParse(roleHeader, out currentUserRole))
                {
                    if (_roles.Count > 0)
                    {
                        foreach (var role in _roles)
                        {

                            if (currentUserRole == (int)role || (int)role == (int)EnumUserType.All)
                            {
                                roleExistsInList = true;
                                break;
                            }
                        }
                    }
                }

                if (_roles.Count > 0 && !string.IsNullOrEmpty(roleHeader))
                {
                    if (roleExistsInList)
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = false;
                        else
                            response = true;
                    }
                    else
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = true;
                        else
                            response = false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(roleHeader))
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = false;
                        else
                            response = true;
                    }
                    else
                    {
                        response = true;
                    }
                }
            }
            catch
            {
                response = true;
            }

            return response;
        }
    }

    public class TempAuthorization : AuthorizationFilterAttribute
    {
        private List<EnumUserType> _roles;
        private RoleSettings _roleSetting;
        private IAccountService _accountService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="roles"></param>
        public TempAuthorization(RoleSettings setting, params EnumUserType[] roles)
        {
            if (_roles == null)
                _roles = new List<EnumUserType>(0);

            foreach (var role in roles)
            {
                _roles.Add(role);
            }
            _roleSetting = setting;
        }

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var rawToken = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                var langId = actionContext.Request.Headers.GetValues("languagePreference").FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(langId))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langId == "2" ? "ar" : "en");
                }


                var token = rawToken.ToString().Substring("Bearer ".Length);
                string secret = "12346578uetstrsi846357825sjsmolfutsknklklfaefhlefblEJF";

                var key = Encoding.ASCII.GetBytes(secret);
                var handler = new JwtSecurityTokenHandler();
                var tokenSecure = handler.ReadToken(token) as SecurityToken;

                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var time = tokenSecure.ValidTo;
                var claims = handler.ValidateToken(token, validations, out tokenSecure);

                var rawRoleHeader = claims.Claims.FirstOrDefault(claim => claim.Type == "typ").Value;

                bool isInvalidUser = false;
                var userGuid = claims.Claims.FirstOrDefault(claim => claim.Type == "jti").Value;
                if (userGuid != null)
                {
                    _accountService = new AccountService();
                    isInvalidUser = _accountService.CheckInvalidUser(Guid.Parse(userGuid));
                }

                bool isInvalidRole = IsInvalidRole(rawRoleHeader);
                if (time.Date < DateTime.UtcNow || isInvalidRole || isInvalidUser)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                }
                else
                {
                    //var claims = handler.ValidateToken(token, validations, out tokenSecure);

                    //string jti = Guid.Empty.ToString();
                    //jti = claims.Claims.First(claim => claim.Type == "jti").Value;
                    //FillUserGuidFromToken(actionContext, jti);
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }

        //private void FillUserGuidFromToken(HttpActionContext actionContext, string guid)
        //{
        //    try
        //    {
        //        string jsonBodyStream;
        //        using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
        //        {
        //            stream.BaseStream.Position = 0;
        //            jsonBodyStream = stream.ReadToEnd();
        //        }
        //        var jsonBody = new StringBuilder();

        //        using (var reader = new StreamReader(jsonBodyStream))
        //        {
        //            jsonBody.Append(reader.ReadToEnd());
        //        }

        //        dynamic jsonStream = JsonConvert.DeserializeObject(jsonBody.ToString());

        //        StringBuilder updatedStream = new StringBuilder();

        //        if (jsonStream != null)
        //        {
        //            jsonStream.userguid = guid;
        //            updatedStream.Append(JsonConvert.SerializeObject(jsonStream));
        //        }
        //        else
        //        {
        //            jsonStream = new
        //            {
        //                userguid = guid
        //            };
        //            updatedStream.Append("{@updatedData}");
        //            updatedStream.Replace("@updatedData", JsonConvert.SerializeObject(jsonStream));
        //        }

        //        var newBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(updatedStream.ToString()));

        //        context.HttpContext.Request.Body = newBodyStream;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private bool IsInvalidRole(string roleHeader)
        {
            bool response = false;

            try
            {
                bool roleExistsInList = false;

                int currentUserRole = -1;

                if (int.TryParse(roleHeader, out currentUserRole))
                {
                    if (_roles.Count > 0)
                    {
                        foreach (var role in _roles)
                        {

                            if (currentUserRole == (int)role || (int)role == (int)EnumUserType.All)
                            {
                                roleExistsInList = true;
                                break;
                            }
                        }
                    }
                }

                if (_roles.Count > 0 && !string.IsNullOrEmpty(roleHeader))
                {
                    if (roleExistsInList)
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = false;
                        else
                            response = true;
                    }
                    else
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = true;
                        else
                            response = false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(roleHeader))
                    {
                        if (_roleSetting == RoleSettings.Allow)
                            response = false;
                        else
                            response = true;
                    }
                    else
                    {
                        response = true;
                    }
                }
            }
            catch
            {
                response = true;
            }

            return response;
        }
    }
    /// <summary>
    /// RoleSettings
    /// </summary>
    public enum RoleSettings
    {
        /// <summary>
        /// Allow
        /// </summary>
        Allow = 1,
        /// <summary>
        /// Restrict
        /// </summary>
        Restrict,
        /// <summary>
        /// Without Auth
        /// </summary>
        NoAction
    }
}
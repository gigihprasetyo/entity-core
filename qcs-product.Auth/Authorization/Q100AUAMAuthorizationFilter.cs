using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using qcs_product.Auth.Authorization.Constants;
using qcs_product.Auth.Authorization.Infrastructure;
using qcs_product.Auth.Authorization.Models;
using qcs_product.Auth.Authorization.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using qcs_product.Auth.Authorization.SettingModels;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using qcs_product.AuthAndEventBus.Authorization.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace qcs_product.Auth.Authorization
{
    public class Q100AUAMAuthorizationFilter : IAsyncActionFilter
    {  
        private readonly IHttpClientFactory _clientFactory;
        private readonly q100_authorizationContext _context;
        private readonly Q100AuthorizationSetting _q100AuthorizationSetting;
        public Q100AUAMAuthorizationFilter(
            q100_authorizationContext context,
            IHttpClientFactory clientFactory,
            IOptions<Q100AuthorizationSetting> q100AuthorizationSetting)
        {
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _q100AuthorizationSetting = q100AuthorizationSetting.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            if (filterContext != null)  
            {  
                Microsoft.Extensions.Primitives.StringValues accessToken;  
                filterContext.HttpContext.Request.Headers.TryGetValue(Q100AUAMAuthorizationConstant.AUTHORIZATION_HEADER, out accessToken);  

                var token = accessToken.FirstOrDefault();  

                if (token != null && token.Contains(Q100AUAMAuthorizationConstant.BEARER))  
                {
                    token = token.Replace(Q100AUAMAuthorizationConstant.BEARER, Q100AUAMAuthorizationConstant.EMPTY_STRING);
                    byte[] tokenKey = Encoding.ASCII.GetBytes(_q100AuthorizationSetting.AccessTokenSecret);
                    TokenValidationViewModel tokenValidationResult = await _ValidateJWTToken(token, tokenKey);
                    if (tokenValidationResult.IsValid)  
                    {
                        string endpointPath = filterContext.HttpContext.Request.Path;

                        if (await _ValidatePath(endpointPath, tokenValidationResult.PositionId))
                        {
                            filterContext.HttpContext.Response.Headers.Add(Q100AUAMAuthorizationConstant.AUTH_STATUS_HEADER, Q100AUAMAuthorizationConstant.AUTHORIZED);
                            await next();                                
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Headers.Add(Q100AUAMAuthorizationConstant.AUTH_STATUS_HEADER, Q100AUAMAuthorizationConstant.NOT_AUTHORIZED);              

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;  
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = Q100AUAMAuthorizationConstant.NOT_AUTHORIZED;  
                            filterContext.Result = new JsonResult(Q100AUAMAuthorizationConstant.NOT_AUTHORIZED)  
                            {  
                                Value = new  
                                {  
                                    Status = 400,  
                                    Message = Q100AUAMAuthorizationConstant.UNAUTHORIZED_ENDPOINT_MESSAGE  
                                },  
                            };
                            return;
                        }
                    }  
                    else  
                    {  
                        filterContext.HttpContext.Response.Headers.Add(Q100AUAMAuthorizationConstant.AUTH_STATUS_HEADER, Q100AUAMAuthorizationConstant.NOT_AUTHORIZED);
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;  
                        filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = Q100AUAMAuthorizationConstant.NOT_AUTHORIZED;  
                        filterContext.Result = new JsonResult(Q100AUAMAuthorizationConstant.NOT_AUTHORIZED)  
                        {  
                            Value = new  
                            {  
                                Status = 401,  
                                Message = Q100AUAMAuthorizationConstant.INVALID_TOKEN_MESSAGE
                            },  
                        };
                        return;
                    } 
                }  
                else  
                {  
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;  
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = Q100AUAMAuthorizationConstant.PLEASE_PROVIDE_AUTHORIZATION;  
                    filterContext.Result = new JsonResult(Q100AUAMAuthorizationConstant.PLEASE_PROVIDE_AUTHORIZATION)  
                    {  
                        Value = new  
                        {  
                            StatusCode = 403,  
                            Message = Q100AUAMAuthorizationConstant.PLEASE_PROVIDE_AUTHORIZATION
                        },  
                    };
                    return;  
                }  
            }
        }

        /// <summary>
        /// validate jwt token
        /// </summary>
        /// <param name="user"></param>
        /// <returns>user data</returns>
        private async Task<TokenValidationViewModel> _ValidateJWTToken(string token, byte[] tokenKey)
        {
            TokenValidationViewModel result = new TokenValidationViewModel();
            try
            {
                if (await _IsAccessTokenActive(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    var userPositionId = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_POSITION_ID_ATTRIBUTE).Value;
                    var userName = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_NAME_ATTRIBUTE).Value;
                    var userEmail = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_EMAIL_ATTRIBUTE).Value;
                    
                    result.IsValid = true;
                    result.Message = Q100AUAMAuthorizationConstant.OK_MESSAGE;
                    result.PositionId = userPositionId;
                    result.Name = userName;
                    result.Email = userEmail;
                }
                else
                {
                    result.IsValid = false;
                    result.Message = Q100AUAMAuthorizationConstant.UNVALID_TOKEN;
                }
            }
            catch 
            {
                result.IsValid = false;
                result.Message = Q100AUAMAuthorizationConstant.UNVALID_TOKEN;
            }
            return result;
        }

        /// <summary>
        /// validate current path
        /// </summary>
        /// <param name="user"></param>
        /// <returns>validation result</returns>
        private async Task<bool> _ValidatePath(string endpointPath, string positionId)
        {
            bool result = false;
            NowTimestamp nowTimestamp = _context.NowTimestamp.FromSqlRaw(Q100AUAMAuthorizationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            List<Endpoint> listAuthorizedEndpoint = await ( 
                from endpoint_data in _context.Endpoint
                where 
                    endpoint_data.BeginDate <= nowTimestamp.CurrentTimestamp && 
                    endpoint_data.EndDate >= nowTimestamp.CurrentTimestamp
                join app_data in _context.Application on
                    endpoint_data.ApplicationCode equals app_data.ApplicationCode
                where 
                    app_data.BeginDate <= nowTimestamp.CurrentTimestamp && 
                    app_data.EndDate >= nowTimestamp.CurrentTimestamp &&
                    app_data.ApplicationCode == _q100AuthorizationSetting.ApplicationCode
                join rte_data in _context.RoleToEndpoint on
                    endpoint_data.EndpointCode equals rte_data.EndpointCode
                where 
                    rte_data.RowStatus == null
                join role_data in _context.Role on
                    rte_data.RoleCode equals role_data.RoleCode
                where 
                    role_data.BeginDate <= nowTimestamp.CurrentTimestamp && 
                    role_data.EndDate >= nowTimestamp.CurrentTimestamp
                join ptr_data in _context.PositionToRole on
                    role_data.RoleCode equals ptr_data.RoleCode
                where 
                    ptr_data.RowStatus == null &&
                    ptr_data.PosId == positionId
                select endpoint_data
            ).ToListAsync();
            foreach (var authorizedEndpoint in listAuthorizedEndpoint)
            {
                var match = Regex.Match(endpointPath, authorizedEndpoint.EndpointPath, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    result = true;
                }             
            }
            return result;
        }

        private async Task<bool> _IsAccessTokenActive(string accessToken)
        {
            var url = "";
            try
            {
                IsAccessTokenActiveViewModel bodyData = new IsAccessTokenActiveViewModel(){
                    AccessToken = accessToken
                };
                var baseUrl = _q100AuthorizationSetting.AUAMServiceURL;
                url = $"{baseUrl}v1/AuthenticatedUser/IsAccessTokenActive";
                string endPoint = $"v1/AuthenticatedUser/IsAccessTokenActive";
                string body = JsonSerializer.Serialize(bodyData);

                HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Post, body);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<HttpResponseMessage> _Perfom(string endPoint, HttpMethod method, string content)
        {
            // var baseUrl = _q100AuthorizationSetting.AUAMServiceURL;
            var baseUrl = _q100AuthorizationSetting.AUAMServiceURL;
            string url = $"{baseUrl}{endPoint}";

            HttpRequestMessage request = new HttpRequestMessage(method, url);

            request.Headers.Add("Accept", "application/json");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            HttpClient client = _clientFactory.CreateClient();

            return await client.SendAsync(request);
        }

        public TokenDecodeViewModel DecodeJwtToken(string accessToken)
        {

            var token = accessToken.Replace(Q100AUAMAuthorizationConstant.BEARER, Q100AUAMAuthorizationConstant.EMPTY_STRING);
            byte[] tokenKey = Encoding.ASCII.GetBytes(_q100AuthorizationSetting.AccessTokenSecret);

            TokenDecodeViewModel result = new TokenDecodeViewModel();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userPositionId = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_POSITION_ID_ATTRIBUTE).Value;
                var userNikId = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_NIK_ATTRIBUTE).Value;
                var userOrganizationId = jwtToken.Claims.First(x => x.Type == Q100AUAMAuthorizationConstant.USER_ORGANIZATION_ATTRIBUTE).Value;

                result.IsValid = true;
                result.Message = Q100AUAMAuthorizationConstant.OK_MESSAGE;
                result.PositionId = userPositionId;
                result.NIK = userNikId;
                result.OrganizationId = userOrganizationId;
            }
            catch
            {
                result.IsValid = false;
                result.Message = Q100AUAMAuthorizationConstant.UNVALID_TOKEN;
            }
            return result;
        }
    }  
}
using ERBus.Entity.Database.Authorize;
using ERBus.Service;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERBus.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var user = new NGUOIDUNG();
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["ERBusConnection"].ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = string.Format(@"SELECT * FROM NGUOIDUNG WHERE USERNAME = '" + context.UserName + "' AND PASSWORD = '" + MD5Encrypt.MD5Hash(context.Password) + "' AND TRANGTHAI = 10 ");
                        using (var oracleDataReader = command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            if (!oracleDataReader.Result.HasRows)
                            {
                                user = null;
                            }
                            else
                            {
                                while (oracleDataReader.Result.Read())
                                {
                                    user.USERNAME = oracleDataReader.Result["USERNAME"]?.ToString();
                                    user.TENNHANVIEN = oracleDataReader.Result["TENNHANVIEN"]?.ToString();
                                    user.SODIENTHOAI = oracleDataReader.Result["SODIENTHOAI"]?.ToString();
                                    user.CHUNGMINHTHU = oracleDataReader.Result["CHUNGMINHTHU"]?.ToString();
                                    user.UNITCODE = oracleDataReader.Result["UNITCODE"]?.ToString();
                                }
                            }
                        }
                    }
                }
                if (user == null)
                {
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng");
                    return;
                }
                Action<ClaimsIdentity, string> addClaim = (ClaimsIdentity obj, string username) => { return; };
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                addClaim.Invoke(identity, user.USERNAME);
                identity.AddClaim(new Claim("unitCode", user.UNITCODE));
                AuthenticationProperties properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                    {
                        "userName", string.IsNullOrEmpty(user.USERNAME)?string.Empty:user.USERNAME
                    },
                    {
                        "fullName", string.IsNullOrEmpty(user.TENNHANVIEN)?string.Empty:user.TENNHANVIEN
                    },
                    {
                        "code", string.IsNullOrEmpty(user.MANHANVIEN)?string.Empty:user.MANHANVIEN
                    },
                    {
                        "phone", string.IsNullOrEmpty(user.SODIENTHOAI)?string.Empty:user.SODIENTHOAI
                    },
                    {
                        "chungMinhThu", string.IsNullOrEmpty(user.CHUNGMINHTHU)?string.Empty:user.CHUNGMINHTHU
                    },
                    {
                        "unitCode", string.IsNullOrEmpty(user.UNITCODE)?string.Empty:user.UNITCODE
                    }
                    });

                AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(identity);
            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", e.Message);
                return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

    }
}
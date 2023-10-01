using PayMasta.Repository.Account;
using PayMasta.Repository.TokenRepository;
using PayMasta.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountRepository _accountRepository;
        public TokenService()
        {
            _tokenRepository = new TokenRepository();
            _accountRepository = new AccountRepository();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(AppSetting.ConnectionStrings);
            }
        }

        public bool IsSessionValid(string token)
        {
            bool res = false;
            var result = _tokenRepository.GetUserSessionByToken(token);
            if (result != null && result.IsActive == true && result.IsDeleted == false)
            {
                var userData = _accountRepository.GetUserByIdForSession(result.UserId);
                if (userData.IsActive == true && userData.IsDeleted == false && userData.Status == 1)
                {
                    res = true;
                }
               
            }

            return res;
        }
    }
}

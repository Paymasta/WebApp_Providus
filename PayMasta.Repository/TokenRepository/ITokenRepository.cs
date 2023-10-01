using PayMasta.DBEntity.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Repository.TokenRepository
{
    public interface ITokenRepository
    {
        UserSession GetUserSessionByToken(string token, IDbConnection exdbConnection = null);
    }
}

using Sabio.Models.Requests;
using System;

namespace Sabio.Services
{
    public interface ITokensService
    {
        int Insert(TokensAddRequest model, int tokenTypeId, Guid token, int userId);
    }
}
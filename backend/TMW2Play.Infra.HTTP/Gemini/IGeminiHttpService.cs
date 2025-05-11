using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMW2Play.Domain.Entities.Steam;

namespace TMW2Play.Infra.HTTP.Gemini
{
    public interface ISteamHttpService
    {
        Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default);
    }
}

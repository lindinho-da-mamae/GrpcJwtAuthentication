using Shared.Requests;
using Shared.Responses;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Shared
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
    }
}
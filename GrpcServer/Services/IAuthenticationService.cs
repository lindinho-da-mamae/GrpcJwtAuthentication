using Shared.Requests;
using Shared.Responses;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
    }
}
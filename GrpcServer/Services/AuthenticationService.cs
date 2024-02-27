

using Grpc.Core;
using Shared.Requests;
using Shared.Responses;

namespace GrpcServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        public Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
         
            AuthenticationResponse authenticationResponse = JwtAuthenticationManager.Authenticate(request);
            if (authenticationResponse == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user Credentials"));
            }
            return Task.FromResult(authenticationResponse);
        }
    }
}





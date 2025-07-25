using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Gender.SoapClients.MVCWebApp.DuyVK.TokenHandlers
{
    // ==============================
    // === Bearer Token Behavior
    // ==============================
    public class BearerTokenBehavior : IEndpointBehavior
    {
        // ==============================
        // === Fields
        // ==============================

        private readonly Func<string> _getToken;

        // ==============================
        // === Cosntructor
        // ==============================

        public BearerTokenBehavior(Func<string> getToken)
        {
            _getToken = getToken;
        }

        // ==============================
        // === Methods
        // ==============================

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new BearerTokenInspector(_getToken));
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Gender.SoapClients.MVCWebApp.DuyVK.TokenHandlers
{
    public class BearerTokenInspector : IClientMessageInspector
    {
        // ==============================
        // === Fields
        // ==============================

        private readonly Func<string> _getToken;

        // ==============================
        // === Constructors
        // ==============================

        public BearerTokenInspector(Func<string> getToken) => _getToken = getToken;

        // ==============================
        // === Methods
        // ==============================

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var http = new HttpRequestMessageProperty();
            http.Headers["Authorization"] = "Bearer " + _getToken();
            request.Properties[HttpRequestMessageProperty.Name] = http;
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState) { }
    }
}

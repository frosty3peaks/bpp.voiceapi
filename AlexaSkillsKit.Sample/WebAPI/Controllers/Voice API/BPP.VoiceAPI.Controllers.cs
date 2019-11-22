using System.Net.Http;
using System.Web.Http;
using BPP.VoiceAPI.Services;

namespace BPP.VoiceAPI.Controllers
{
   
    [RoutePrefix("bppvoice")]

    //Alexa
    public class BPPVoiceController : ApiController
    {       
        [Route("alexa")]
        [HttpPost]
        public HttpResponseMessage alexa()
        {
            var speechlet = new AlexaSpeechletService();
            return speechlet.GetResponse(Request);
        }

        [Route("cortana")]
        [HttpPost]
        public HttpResponseMessage cortana()
        {
            var _response = new HttpResponseMessage();
            _response.StatusCode = System.Net.HttpStatusCode.Unused;
            _response.ReasonPhrase = string.Format("Sorry, BPP Voice has not implemented {0} Interface. Watch this space!", "a Cortana");            
            return _response;
        }

        [Route("siri")]
        [HttpPost]
        public HttpResponseMessage siri()
        {
            var _response = new HttpResponseMessage();
            _response.StatusCode = System.Net.HttpStatusCode.Unused;
            _response.ReasonPhrase = string.Format("Sorry, BPP Voice has not implemented {0} Interface. Watch this space!", "a Siri");
            return _response;
        }

        [Route("okgoogle")]
        [HttpPost]
        public HttpResponseMessage okgoogle()
        {
            var _response = new HttpResponseMessage();
            _response.StatusCode = System.Net.HttpStatusCode.Unused;
            _response.ReasonPhrase = string.Format("Sorry, BPP Voice has not implemented {0} Interface. Watch this space!", "an OK Google");
            return _response;
        }

    }

    
}

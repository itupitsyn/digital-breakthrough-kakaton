using Hencoder.Responces;
using Hencoder.Services;
using Hencoder.Services.RecomendationSystem;
using Microsoft.AspNetCore.Mvc;
using ZeroLevel;

namespace Hencoder.Controllers
{
    [ApiController]
    public class AuthController
       : BaseController
    {
        private readonly RecSys _recSys;
        public AuthController(RecSys recSys)
            : base()
        {
            _recSys = recSys;
        }

        [HttpPost("/api/auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AutoCodeResponse> SignIn()
        {
            if (string.IsNullOrWhiteSpace(OperationContext.CurrentUserToken) == false)
            {
                if (_recSys.VerifyToken(OperationContext.CurrentUserToken))
                {
                    Log.Debug("[AuthController.SignIn] The token has already been issued.");
                    return new ActionResult<AutoCodeResponse>(new AutoCodeResponse { Token = OperationContext.CurrentUserToken, Success = true });
                }
                else
                {
                    Log.Debug("[AuthController.SignIn] Unknown token transferred.");
                }
            }
            var token = _recSys.CreateAccount();
            Log.Debug($"[AuthController.SignIn] A new token has been created. {token}");
            return new ActionResult<AutoCodeResponse>(new AutoCodeResponse { Token = token, Success = true });
        }
    }
}

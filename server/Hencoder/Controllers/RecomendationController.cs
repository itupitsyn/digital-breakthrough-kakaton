using Hencoder.Models;
using Hencoder.Services;
using Hencoder.Services.RecomendationSystem;
using Microsoft.AspNetCore.Mvc;
using ZeroLevel;

namespace Hencoder.Controllers
{
    [Route("api/recsys")]
    [ApiController]
    public class RecomendationController
        : BaseController
    {
        private readonly RecSys _recSys;

        public RecomendationController(RecSys recSys)
        {
            _recSys = recSys;
        }

        [HttpPost("next")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<VideoStatEntry>>> Next()
        {
            if (string.IsNullOrWhiteSpace(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Next] Missing token.");
                return BadRequest();
            }
            if (false == _recSys.VerifyToken(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Next] An invalid token was passed.");
                return BadRequest();
            }
            var recomendation = (await _recSys.GetNextRecomendation(OperationContext.CurrentUserToken)).ToArray();
            if (recomendation != null && recomendation.Length > 0)
            {
                _recSys.SaveUserRecomendation(OperationContext.CurrentUserToken, recomendation);
                Log.Warning($"[RecomendationController.Next] A list of recommendations has been received VideoIDS [{string.Join(", ", recomendation.Select(r => r.video_id))}].");
            }
            else
            {
                Log.Warning($"[RecomendationController.Next] Failed to retrieve the recommendation list for the token '{OperationContext.CurrentUserToken}'.");
            }            
            return Ok(recomendation);
        }

        [HttpPost("like/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Like([FromRoute] long id)
        {
            if (string.IsNullOrWhiteSpace(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Like] Missing token.");
                return BadRequest();
            }
            if (false == _recSys.VerifyToken(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Like] An invalid token was passed.");
                return BadRequest();
            }
            var action = new RSUserAction
            {
                action_type = (int)RSUserActionType.Like,
                timestamp = Timestamp.UtcNow,
                video_id = id            };
            var updated = _recSys.SaveUserAction(OperationContext.CurrentUserToken, action);
            Log.Debug($"[RecomendationController.Like] Records updated: {updated}.");
            return Ok();
        }

        [HttpPost("skip/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Skip([FromRoute] long id)
        {
            if (string.IsNullOrWhiteSpace(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Skip] Missing token.");
                return BadRequest();
            }
            if (false == _recSys.VerifyToken(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Skip] An invalid token was passed.");
                return BadRequest();
            }
            var action = new RSUserAction
            {
                action_type = (int)RSUserActionType.Ignored,
                timestamp = Timestamp.UtcNow,
                video_id = id
            };
            var updated = _recSys.SaveUserAction(OperationContext.CurrentUserToken, action);
            Log.Debug($"[RecomendationController.Skip] Records updated: {updated}.");
            return Ok();
        }

        [HttpPost("dislike/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Dislike([FromRoute] long id)
        {
            if (string.IsNullOrWhiteSpace(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Dislike] Missing token.");
                return BadRequest();
            }
            if (false == _recSys.VerifyToken(OperationContext.CurrentUserToken))
            {
                Log.Debug($"[RecomendationController.Dislike] An invalid token was passed.");
                return BadRequest();
            }
            var action = new RSUserAction
            {
                action_type = (int)RSUserActionType.Dislike,
                timestamp = Timestamp.UtcNow,
                video_id = id
            };
            var updated = _recSys.SaveUserAction(OperationContext.CurrentUserToken, action);
            Log.Debug($"[RecomendationController.Dislike] Records updated: {updated}.");
            return Ok();
        }

        [HttpGet("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Test()
        {
            var data = _recSys.GetTestData();
            return Ok(data);
        }
    }
}

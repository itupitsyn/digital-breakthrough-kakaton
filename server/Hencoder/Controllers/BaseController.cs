using Hencoder.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hencoder.Controllers
{
    public abstract class BaseController
       : Controller
    {
        public BaseController()
        {
        }

        protected OperationContext OperationContext
        {
            get
            {
                return (HttpContext.Items["op_context"] as OperationContext)!;
            }
        }
    }
}

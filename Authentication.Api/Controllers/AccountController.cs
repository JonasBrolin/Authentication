using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;

using Authentication.API.Entity;
using Authentication.API.Models;
using Authentication.API.Repository;

using Microsoft.AspNet.Identity;

namespace Authentication.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly AuthRepository repository;

        public AccountController()
        {
            this.repository = new AuthRepository();
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result = await this.repository.RegisterUser(userModel);

            IHttpActionResult errorResult = this.GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return this.Ok();
        }

        [AllowAnonymous]
        [Route("AddClient")]
        public async Task<IHttpActionResult> AddClient(ClientModel clientModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.repository.AddClient(clientModel);

            //IHttpActionResult errorResult = this.GetErrorResult(result);

            if (!result)
            {
                return this.BadRequest("Did not work to save client");
            }

            return this.Ok();
        }

        [AllowAnonymous]
        [Route("Delete/{userName}")]
        [HttpPost]
        public async Task<IHttpActionResult> Delete(string userName)
        {
            IdentityResult result = await this.repository.DeleteUser(userName);

            IHttpActionResult errorResult = this.GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return this.Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}

using coffee.Api.Infrastructure;
using coffee.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace coffee.Api.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Email,
                Email = createUserModel.Email,
                Level = 3,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            AppUserManager.AddToRole(user.Id, "User");

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));

        }

        //confirm email route, if the user clicks the confirmation link in the e-mail address, this method will be invoked
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<HttpResponseMessage> ConfirmEmail(string userId = "", string code = "")
        {
            var response = new HttpResponseMessage();
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                response.Content = new StringContent("<html><body><h1 style='text-align: center;'>User Id and Code are required</h1></body></html>");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
               
                response.Content = new StringContent("<html><body><h1 style='text-align: center;'>Registration successful</h1></body></html>");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return  response;
            }
            else
            {
                response.Content = new StringContent("<html><body><h1 style='text-align: center;'>Error, please try again, or register with another email.</h1></body></html>");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                return response;
            }
        }

        //change password 
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        [AllowAnonymous]
        [Route("SendPasswordReset")]
        [HttpGet]
        public async Task<IHttpActionResult> SendPasswordReset(string email)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                if (email == null)
                {
                    return BadRequest();
                }

                var user = await this.AppUserManager.FindByEmailAsync(email);

                if (user == null)
                {

                    return NotFound();
                }
                var resetToken = this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);

                await this.AppUserManager.SendEmailAsync(user.Id, "Confirmation code", $"Your code:{resetToken.Result}");


                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }
        }

        [AllowAnonymous]
        [Route("ProvideToken")]
        [HttpGet]
        public async Task<IHttpActionResult> ProvideToken(string token, string email, string newPass)
        {
            try
            {
                var db = ApplicationDbContext.Create();

                var user = await this.AppUserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return NotFound();
                }

                var code = token.Replace(" ", "+");
               
                IdentityResult passwordChangeResult = await AppUserManager.ResetPasswordAsync(user.Id, code, newPass);

                if (!passwordChangeResult.Succeeded)
                {
                    return GetErrorResult(passwordChangeResult);
                }

                return Ok("Password changed");
            }
            catch (Exception e)
            {

                return BadRequest(e.ToString());
            }

        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {


            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
    }
}


using System.Threading.Tasks;
using AngularASPNETCore2WebApiAuth.Data;
using AngularASPNETCore2WebApiAuth.Helpers;
using AngularASPNETCore2WebApiAuth.Models.Entities;
using AngularASPNETCore2WebApiAuth.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AngularASPNETCore2WebApiAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AngularASPNETCore2WebApiAuth.Controllers
{
    [Route("api/[controller]")] 
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;


        public AccountsController(IEmailSender sender, UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
           _accessor = httpContextAccessor;
        }

    // POST api/accounts
    [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }


    // Post api/accounts/reset
    [Authorize]
    [HttpPost("reset")]
    public async Task<IActionResult> Post([FromBody]CredentialsViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var _caller = _accessor.HttpContext.User;
      var _userId = _caller.Claims.Single(c => c.Type == "id").Value;

      var userIdentity = await _userManager.FindByIdAsync(_userId); //_mapper.Map<AppUser>(model);

      await _userManager.RemovePasswordAsync(userIdentity);
      await _userManager.AddPasswordAsync(userIdentity, model.Password);
      //await _userManager.ResetPasswordAsync()

      return new OkObjectResult("Password reset");
    }


    /*
    // POST api/accounts
    [HttpPost]
    public async Task<IActionResult> Reset([FromBody]RegistrationViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdentity = _mapper.Map<AppUser>(model);

      var result = await _userManager.CreateAsync(userIdentity, model.Password);

      if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

      await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
      await _appDbContext.SaveChangesAsync();

      return new OkObjectResult("Account created");
    }
    */

  }

}


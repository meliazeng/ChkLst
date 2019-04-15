using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AngularASPNETCore2WebApiAuth.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AngularASPNETCore2WebApiAuth.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AngularASPNETCore2WebApiAuth.ViewModels;
using AngularASPNETCore2WebApiAuth.Extensions;
using AngularASPNETCore2WebApiAuth.Services;

namespace AngularASPNETCore2WebApiAuth.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]/[action]")]
  public class TasksController : Controller
  {
    private readonly string _userId;
    private readonly ApplicationDbContext _appDbContext;
    private readonly DateTime _currentDate;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailSender _sender;

    public TasksController(IEmailSender sender, UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
      _userManager = userManager;
      _sender = sender;
      var _caller = httpContextAccessor.HttpContext.User;
      if (_caller.Identity.IsAuthenticated)
        _userId = _caller.Claims.Single(c => c.Type == "id").Value;
      _appDbContext = appDbContext;
      _currentDate = DateTime.Now;
    }

    [HttpGet]
    public async Task<IActionResult> TasksSummary()
    {
      var _currentDate = System.DateTime.Now;
      var result = await _appDbContext.CTasks.Where(c => c.SDate.Date == _currentDate.Date).ToArrayAsync();
      var users = result.GroupBy(c => c.IdentityId).Select(g => new { g.Key });
      foreach (var userId in users)
      {
        var user = await _userManager.FindByIdAsync(userId.Key);
        //var user = _appDbContext.Users.Where(x=> x.Id == userId.ToString()).First();
        var completeTasks = await _appDbContext.CTasks.Where(c => c.IdentityId == userId.Key && c.SDate.Date == _currentDate.Date && c.Status == true).ToArrayAsync();
        var outstandingTasks = await _appDbContext.CTasks.Where(c => c.IdentityId == userId.Key && c.SDate.Date == _currentDate.Date && c.Status != true).ToArrayAsync();

        string message = "<p>Checked Task:</p><oi>";
        foreach (var task in completeTasks)
        {
          message = message + "<li>" + task.Descr + "</li>";
        }
        message = message + "</oi><p>Unchecked Tasks:</p><oi>";

        foreach (var task in outstandingTasks)
        {
          message = message + "<li>" + task.Descr + "</li>";
        }
        message = message + "</oi>";
        await _sender.SendEmailAsync(user.UserName, "Summary of " + _currentDate.Date.ToShortDateString(), message);
      }

      return new OkObjectResult(new { });
    }

    [Authorize(Policy = "ApiUser")]
    [HttpGet]
    public async Task<IActionResult> GetTasks(bool isCheckin) {
      if (isCheckin)
      {
        var result = await _appDbContext.CTasks.Where(c => c.IdentityId == _userId && c.SDate.Date == _currentDate.Date).ToArrayAsync();
        return new OkObjectResult(result.Where(c => c.SettingID == null));
      }
      else
      {
        var result = await _appDbContext.CTasks.Where(c => c.IdentityId == _userId && c.SDate.Date == _currentDate.Date).ToArrayAsync();
        return new OkObjectResult(result.Select(x=> new TaskViewModel { cTaskId = x.CTaskId, descr = x.Descr, note = x.Note, status= x.Status }));
      }
    }
    
    [Authorize(Policy = "ApiUser")]
    [HttpPost]
    public async Task<IActionResult> UpdateTask([FromBody]TaskViewModel model)
    {
      int? id = model.cTaskId;
      if (model.cTaskId == null)
      {
        var task = await _appDbContext.CTasks.AddAsync(new CTask { IdentityId = _userId, Descr = model.descr, Note = model.note, SDate = _currentDate, Status = model.status });
        await _appDbContext.SaveChangesAsync();
        id = task.Entity.CTaskId;    
      }
      else
      {
        var task = _appDbContext.CTasks.Find(model.cTaskId);
        task.Descr = model.descr;
        task.Note = model.note;
        task.Status = model.status;
        _appDbContext.CTasks.Update(task);
        await _appDbContext.SaveChangesAsync();
      }

      return new OkObjectResult(new { id = id });
    }

    [Authorize(Policy = "ApiUser")]
    [HttpPost]
    public async Task<IActionResult> UpdateTasks([FromBody] TasksView model)
    {

      if (model.model != null)
      {
        foreach(TaskViewModel task in model.model)
        {
          var item = _appDbContext.CTasks.Find(task.cTaskId);
          item.Descr = task.descr;
          item.Status = task.status;
          item.Note = task.note;
          _appDbContext.CTasks.Update(item);
        }
     
        await _appDbContext.SaveChangesAsync();
      }

      return new OkObjectResult(new { message = "success!" });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTasksGen([FromBody] TasksView model)
    {

      if (model.model != null)
      {
        foreach (TaskViewModel task in model.model)
        {
          var item = _appDbContext.CTasks.Find(task.cTaskId);
          item.Descr = task.descr;
          item.Status = task.status;
          item.Note = task.note;
          _appDbContext.CTasks.Update(item);
        }

        await _appDbContext.SaveChangesAsync();
      }

      return new OkObjectResult(new { message = "success!" });
    }

    [Authorize(Policy = "ApiUser")]
    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
      var settings = await _appDbContext.Settings.Include("RepeatPattern").Where(c => c.IdentityId == _userId && (c.EndDate == DateTime.MinValue || c.EndDate.Value.Date >= _currentDate.Date)).ToArrayAsync();
      //var patterns = await _appDbContext.RepeatPatterns.ToArrayAsync();
      //_appDbContext.Entry(settings).Reference("RepeatPattern").Load();

      return new OkObjectResult(settings.Select(x => new
      {
        EndDate = (x.EndDate == DateTime.MinValue) ? null : x.EndDate,
        Descr = x.Descr,
        SettingId = x.SettingId,
        RepeatPatternId = x.RepeatPatternId,
        RepeatDescr = x.RepeatPattern.Descr
      }).ToArray());
    }

    [Authorize(Policy = "ApiUser")]
    [HttpPost]
    public async Task<IActionResult> UpdateSetting([FromBody]SettingViewModel model)
    {
      if (model.RepeatPatternId == 0)
        model.RepeatPatternId = 1;

      if (model.EndDate == null)
        model.EndDate = DateTime.MinValue;

      int? id = model.Id;
      if (model.Id == null)
      {
        var setting = await _appDbContext.Settings.AddAsync(new Setting { IdentityId = _userId, Descr = model.Descr,  StartDate = _currentDate, EndDate = (model.EndDate == DateTime.MinValue && model.RepeatPatternId == 11)? _currentDate.AddDays(1)  : model.EndDate, RepeatPatternId = model.RepeatPatternId });
        try
        {
          await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
          return new NotFoundObjectResult(new { Message = ex.Message });
        }

        id = setting.Entity.SettingId;
      }
      else
      {
        var setting = _appDbContext.Settings.Find(model.Id);
        setting.Descr = model.Descr;
        setting.EndDate = (model.EndDate == DateTime.MinValue && model.RepeatPatternId == 11)? _currentDate.AddDays(1) : model.EndDate;
        setting.RepeatPatternId = model.RepeatPatternId;
        _appDbContext.Settings.Update(setting);
        await _appDbContext.SaveChangesAsync();
      }

      return new OkObjectResult(new { id = id });
    }

    [Authorize(Policy = "ApiUser")]
    [HttpGet]
    public async Task<IActionResult> GetPatterns()
    {
      var patterns = await _appDbContext.RepeatPatterns.ToArrayAsync();
      return new OkObjectResult(patterns);
    }

    [Authorize(Policy = "ApiUser")]
    [HttpDelete]
    public async Task<IActionResult> DeleteSetting(int id)
    {
      var relatedTasks = _appDbContext.CTasks.Where(c => c.SDate.Date == _currentDate.Date && c.SettingID == id);

      foreach (CTask task in relatedTasks)
      {
        _appDbContext.CTasks.Remove(task);
      }

      //_appDbContext.CTasks.Remove(_appDbContext.CTasks.Where(c => c.SDate.Date == _currentDate.Date && c.SettingID == id).Single());
      _appDbContext.Settings.Remove(_appDbContext.Settings.Find(id));
      await _appDbContext.SaveChangesAsync();
      return new OkObjectResult( new { message = "success!"});
    }

    [Authorize(Policy = "ApiUser")]
    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
      _appDbContext.CTasks.Remove(_appDbContext.CTasks.Find(id));
      await _appDbContext.SaveChangesAsync();
      return new OkObjectResult(new { message = "success!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetTasksForUser(string userId)
    {
        var result = await _appDbContext.CTasks.Where(c => c.IdentityId == userId && c.SDate.Date == _currentDate.Date).ToArrayAsync();
        return new OkObjectResult(result);
    }


  }
}

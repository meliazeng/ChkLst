
using AngularASPNETCore2WebApiAuth.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace AngularASPNETCore2WebApiAuth.Extensions
{
  public static class GeneralExtensions
  {
    public static bool isValid(this Setting setting, DateTime? _date = null)
    {
      var date = DateTime.Now.Date; 
      if (_date != null)
        date = _date.Value.Date;
      var sdate = setting.StartDate.Date;
      switch(setting.RepeatPattern.Func)
      {
        case 1: //every Year
          return sdate.Month == date.Date.Month && sdate.Day == date.Day;
        case 2: //every Month
          return sdate.Day == date.Day;
        case 3: //every week
          return sdate.DayOfWeek == date.DayOfWeek;
        case 4: //every 2 day.
          return (date - sdate).TotalDays % 2 == 0;
        case 5: //every 3 day.
          return (date - sdate).TotalDays % 3 == 0;
        case 6: //every 4 day.
          return (date - sdate).TotalDays % 4 == 0;
        case 7: //every 5 day.
          return (date - sdate).TotalDays % 5 == 0;
        case 8: //every 6 day.
          return (date - sdate).TotalDays % 6 == 0;
        default:
          return true;
      }

    }
  }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class Setting
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SettingId { get; set; }

    public string IdentityId { get; set; }
    public string Descr { get; set; }
    public DateTime? EndDate { get; set; }
    public int RepeatPatternId { get; set; }
    public RepeatPattern RepeatPattern{ get; set; } // navigation property
    public DateTime StartDate { get; set; }
  }
}

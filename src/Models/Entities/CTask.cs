using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularASPNETCore2WebApiAuth.Models.Entities
{
  public class CTask
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CTaskId { get; set; }

    public string IdentityId { get; set; }
    public string Descr { get; set; }
    public int? SettingID { get; set; }
    public bool? Status { get; set; }
    public string Note { get; set; }
    public DateTime SDate { get; set; }
  }
}

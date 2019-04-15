using System;

namespace AngularASPNETCore2WebApiAuth.ViewModels
{
    public class SettingViewModel
    {
        public int? Id { get; set; }
        public string Descr { get; set; }
        public DateTime? EndDate { get; set; }
        public int RepeatPatternId { get; set; }
    }
}

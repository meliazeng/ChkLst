

namespace AngularASPNETCore2WebApiAuth.ViewModels
{
    public class TaskViewModel
    {
        public int? cTaskId { get; set; }
        public string descr { get; set; }
        public bool? status { get; set; }
        public string note { get; set; }
    }

    public class TasksView
    {
      public TaskViewModel[] model { get; set; }
    }
}

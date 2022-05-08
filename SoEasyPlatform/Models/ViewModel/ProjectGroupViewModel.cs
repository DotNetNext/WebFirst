namespace SoEasyPlatform.Models.ViewModel
{
    public class ProjectGroupViewModel : PageViewModel, IView
    {
        [PropertyName("编号")]
        public int? Id { get; set; }
        [ValidateReduired()]
        [PropertyName("名称")]
        public string Name { get; set; }
        [ValidateReduired()]
        [PropertyName("统一路径,会替换私有路径")]
        public string SolutionPath { get; set; }
        [ValidateReduired()]
        [PropertyName("方案集合")]
        public string ProjectNames { get; set; }
        [ValidateReduired()]
        public int? Sort { get; set; }
    }

    public class ProjectGroupGridViewModel
    {
        [DisplayName("编号")]
        public int? Id { get; set; }
        [DisplayName("名称")]
        public string Name { get; set; }
        [DisplayName("统一路径")]
        public string SolutionPath { get; set; }
        [DisplayName("方案集合")]
        public string ProjectNames { get; set; }
    }
}

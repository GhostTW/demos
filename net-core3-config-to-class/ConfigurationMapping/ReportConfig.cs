using System.Collections.Generic;

namespace ConfigurationMapping
{
    public class ReportConfig
    {
        public ReportSectionConfig ReportSheet { get; set; }
    }
    public class ReportSectionConfig
    {
        public string OutPutPath { get; set; }
        public string ProjectsPackagesList { get; set; }
        public string PackageInfoList { get; set; }
    }

    public class RootConfig
    {
        public SectionConfig Section0 { get; set; }
        public SectionConfig Section1 { get; set; }
    }

    public class SectionConfig
    {
        public string Key0 { get; set; }
        public string Key1 { get; set; }
    }

    public class CommandlineArgumentsConfig
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Mail { get; set; }
        public string Comment { get; set; }
    }
}

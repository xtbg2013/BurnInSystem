using System.ComponentModel;

namespace BurnInUI
{
    public class ProductInformation
    {
        [DisplayName("Serial Number"),ReadOnly(true)]
        public string SerialNumber { get; set; } = "";

        [DisplayName("Plan"), ReadOnly(true)]
        public string Plan { get; set; } = "";

        [DisplayName("Board Name"), ReadOnly(true)]
        public string BoardName { get; set; } = "";
        
        [DisplayName("Board Seat"), ReadOnly(true)]
        public string BoardSeat { get; set; } = "";

        [DisplayName("Cost Time(min)"), ReadOnly(true)]
        public string CostTime { get; set; } = "";

        [DisplayName("Time Remain(min)"), ReadOnly(true)]
        public string RemainTime { get; set; } = "";

        [DisplayName("Unit State"), ReadOnly(true)]
        public string UnitState { get; set; } = "";
        
        [DisplayName("Unit Result"), ReadOnly(true)]
        public string UnitResult { get; set; } = "";

        [DisplayName("Create Time"), ReadOnly(true)]
        public string CreateTime { get; set; } = "";

        [DisplayName("Last Monitor Time"), ReadOnly(true)]
        public string LastMonitorTime { get; set; } = "";
        
    }
}

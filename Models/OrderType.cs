using System.ComponentModel;

namespace MYPM.Models
{
    public enum OrderType
    {
        [Description("Dine In")]
        DineIn,
        [Description("Carry Out")]
        CarryOut,
        [Description("Delivery")]
        Delivery
    }
}


using System;
using System.ComponentModel;
using System.Reflection;

namespace MauiApp1.Models
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


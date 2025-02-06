using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIOT.SemanticKernel.Plugins
{
    public class TimesPlugin
    {
        [KernelFunction]
        [Description("返回当前时间")]
        public async Task<string> GetCurrTimeAsync()
        {
            Console.WriteLine("【本机函数】-返回当前时间-GetCurrTimeAsync");

            var result = DateTime.Now;

            return result.ToString();
        }

        [KernelFunction]
        [Description("返回当前日期")]
        public async Task<string> GetCurrDateAsync()
        {
            Console.WriteLine("【本机函数】-返回当前日期-GetCurrDateAsync");
            var result = DateTime.Now.Date;

            return result.ToString();
        }

        [KernelFunction]
        [Description("返回当前月份")]
        public async Task<string> GetCurrMonthsAsync()
        {
            Console.WriteLine("【本机函数】-返回当前月份-GetCurrMonthsAsync");
            var result = DateTime.Now.Month;

            return result.ToString();
        }

        [KernelFunction]
        [Description("返回当前星期几")]
        public async Task<string> GetCurrWeekAsync()
        {
            Console.WriteLine("【本机函数】-返回当前星期几-GetCurrWeekAsync");
            var result = DateTime.Now.DayOfWeek;

            return result.ToString();
        }

        [KernelFunction]
        [Description("计算指定时间分钟数量")]
        [return: Description("时间分钟数")]
        public async Task<string> CalculateTimeHour([Description("时间小时数")] int hour, [Description("时间分钟数")] int minute)
        {
            Console.WriteLine("【本机函数】-计算指定时间分钟数量-GetCurrWeekAsync");
            var result = hour * 60 + minute;

            return result.ToString();
        }
    }
}

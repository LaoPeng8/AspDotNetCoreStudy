using System.Text.RegularExpressions;

namespace _02_RoutingExample.CustomerConstraint
{
    // 实现 IRouteConstraint 接口, 表示该类为自定义路由约束类
    public class MonthsCustomerConstraint : IRouteConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext">请求上下文 可获取 request response</param>
        /// <param name="route">请求的路由, 即约束应用的路由</param>
        /// <param name="routeKey">路由key, 即 "/payment-report/{year:int:min(1900)}/{month:mymonths}" 中的 month 因此约束实际应用在month路由参数上</param>
        /// <param name="values">路由值, 即 year mont等routeKey的键值对 例 [{year:2025},{month:one}]</param>
        /// <param name="routeDirection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // 如果字典中不包含 约束的路由key则相当于接口请求时没有输入, 则直接false
            if(!values.ContainsKey(routeKey))
            {
                return false;
            }

            // 执行约束逻辑
            Regex regex = new Regex("^(one|two|three|four)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if (regex.IsMatch(monthValue))
            {
                return true;
            }

            return false;
        }
    }
}

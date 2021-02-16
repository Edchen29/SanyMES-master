using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 订单接口扩展模型
	/// </summary>
    public partial class InterfaceOrderModel
    {
        /// <summary>
        /// 订单接口主表
        /// </summary>
        public InterfaceOrderHeader interfaceOrderHeader;
        /// <summary>
        /// 订单接口子表
        /// </summary>
        public List<InterfaceOrderDetiail> interfaceOrderDetails;
    }
}
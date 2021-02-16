using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 产品接口扩展模型
	/// </summary>
    public partial class InterfaceProductModel
    {
        /// <summary>
        /// 产品接口主表
        /// </summary>
        public InterfaceProductHeader interfaceProductHeader;
        /// <summary>
        /// 产品接口子表
        /// </summary>
        public List<InterfaceProductDetail> interfaceProductDetails;
    }
}
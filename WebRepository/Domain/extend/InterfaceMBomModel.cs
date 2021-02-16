using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 制造BOM接口扩展模型
	/// </summary>
    public partial class InterfaceMBomModel
    {
        /// <summary>
        /// 生产BOM接口主表
        /// </summary>
        public InterfaceMbomHeader interfaceMbomHeader;
        /// <summary>
        /// 生产BOM接口子表
        /// </summary>
        public List<InterfaceMbomDetail> interfaceMbomDetails;
    }
}
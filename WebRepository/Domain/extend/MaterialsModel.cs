using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
	/// 物料
	/// </summary>
    public partial class MaterialsModel
    {
        /// <summary>
        /// 物料数据列表
        /// </summary>
        public List<Material> Data;
    }
}
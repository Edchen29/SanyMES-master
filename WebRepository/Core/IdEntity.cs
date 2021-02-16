using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRepository
{
    /// <summary>
    /// Id实体
    /// </summary>
    public abstract class IdEntity
    {
        /// <summary>
	    /// Id
	    /// </summary>
        [Column("Id")]
        public int? Id { get; set; }

    }
}

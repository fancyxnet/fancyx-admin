using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.EfCore.BaseEntity
{
    public interface ITreeEntity
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [Column("parent_id")]
        Guid? ParentId { get; set; }

        /// <summary>
        /// 树形路径
        /// </summary>
        [StringLength(1024)]
        [Column("tree_path")]
        string TreePath { get; set; }

        /// <summary>
        /// 树形层级
        /// </summary>
        [DefaultValue(0)]
        [Column("tree_level")]
        int TreeLevel { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public abstract class BaseTable
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }
    
    [Column("created_time")]
    public DateTime CreatedDate { get; set; }
    
    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
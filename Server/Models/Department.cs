using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("tb_m_departments")]
public class Department : BaseTable
{
    [Column("name")]
    public string Name { get; set; }

    [Column("code")]
    public string Code { get; set; }
        
    public virtual ICollection<Employee> Employee { set; get; }
}
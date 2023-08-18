using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("tb_m_roles")]
public class Role : BaseTable
{
    [Column("name")]
    public string Name { get; set; }

    public virtual ICollection<AccountRole> AccountRole { get; set; }
}
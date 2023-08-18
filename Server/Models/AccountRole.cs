using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("tb_tr_account_roles")]
public class AccountRole : BaseTable
{
    [Column("account_guid")]
    [ForeignKey("Account")]
    public Guid AccountGuid { get; set; }
    
    [Column("account_guid")]
    [ForeignKey("Role")]
    public Guid RoleGuid { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual Role Role { get; set; }
}
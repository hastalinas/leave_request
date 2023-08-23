using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Server.Models;

[Table("tb_m_accounts")]
public class Account : BaseTable
{
    [Key] 
    [Column("guid")]
    [ForeignKey("Employee")]
    public Guid Guid { get; set; }
    
    [Column("profil_picture")] 
    public string? ProfilPictureUrl { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("otp")]
    public int Otp { get; set; }
    
    [Column("is_used")]
    public bool IsUsed { get; set; }
    
    [Column("expired_time")]
    public DateTime? ExpiredTime { get; set; }
        
    public virtual Employee Employee { get; set; }
    public virtual ICollection<AccountRole> AccountRole { get; set; }
}
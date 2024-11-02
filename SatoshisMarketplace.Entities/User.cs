using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [MaxLength(64)] // 64 bytes for hash of SHA-512
        public byte[] PasswordHash { get; set; }

        #region Navigational Properties

        public ICollection<UserLog> UserLogs { get; set; } = new HashSet<UserLog>();

        #endregion
    }
}

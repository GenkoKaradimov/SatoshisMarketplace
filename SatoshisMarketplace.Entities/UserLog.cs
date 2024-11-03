using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class UserLog
    {
        [Key, Column(Order = 1)]
        [Required]
        public DateTime Timestamp { get; set; }

        [StringLength(39)] 
        public string IP { get; set; }

        public UserLogType Type { get; set; }

        #region Navigational Properties

        [Key, Column(Order = 0)]
        [ForeignKey("User")]
        [Required]
        public string Username { get; set; }

        public User User { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BrewersBuddy.Models
{
	[Table("Friend")]
	public class Friend
	{
		[Key]
		public int FriendId { get; set; }
		[Required]
		public int UserId { get; set; }

		public int FriendUserId { get; set; }

        [ForeignKey("FriendUserId")]
        public virtual UserProfile FriendUser { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
	}
}
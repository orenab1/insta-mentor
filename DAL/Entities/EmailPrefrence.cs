using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DAL.Entities
{
    [Table("EmailPrefrences")]
    public class EmailPrefrence
    {
        [Key]
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }

        public bool QuestionAskedOnMyTags { get; set; }
        public bool OnlyNotifyOnCommunityQuestionAsked { get; set; }
        public bool MyQuestionReceivedNewOffer { get; set; }
        public bool MyQuestionReceivedNewComment { get; set; }

        public AppUser AppUser { get; set; }

    }
}
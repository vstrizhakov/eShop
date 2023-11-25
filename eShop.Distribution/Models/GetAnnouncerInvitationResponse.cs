using eShop.Distribution.Models;

namespace eShop.Distribution.Models
{
    public class GetAnnouncerInvitationResponse
    {
        public Account Announcer { get; set; }
        public InvitationLinks Links { get; set; }
    }
}

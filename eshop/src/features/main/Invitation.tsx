import { useGetInvitationLinkQuery } from "../api/telegramSlice";
import { useGetViberInviteLinkQuery } from "../api/viberSlice";

const Invitation: React.FC = () => {
    const {
        data: telegramInvitation,
    } = useGetInvitationLinkQuery(undefined);

    const {
        data: viberInvitation,
    } = useGetViberInviteLinkQuery(undefined);

    
    return (
        <>
            {telegramInvitation && (
                <div>
                    Telegram Invitation: <a target="_blank" href={telegramInvitation.inviteLink}>{telegramInvitation.inviteLink}</a>
                </div>
            )}
            {viberInvitation && (
                <div>
                    Viber Invitation: <a target="_blank" href={viberInvitation.inviteLink}>{viberInvitation.inviteLink}</a>
                </div>
            )}
        </>
    );
};

export default Invitation;
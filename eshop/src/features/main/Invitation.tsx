import { useParams } from "react-router-dom";
import { Card, Spinner } from "react-bootstrap";
import { ReactComponent as Viber } from "../../assets/viber.svg";
import { ReactComponent as Telegram } from "../../assets/telegram.svg";
import { useGetAnnouncerInvitationQuery } from "../api/distributionSlice";

const Invitation: React.FC = () => {
    const {
        providerId,
    } = useParams();

    const {
        isLoading,
        data: invitation,
    } = useGetAnnouncerInvitationQuery(providerId!);


    return (
        <div className="d-flex flex-column align-items-center justify-content-center mt-5">
            {isLoading && (
                <Spinner />
            )}
            {invitation && (
                <Card>
                    <Card.Body className="d-flex gap-4 p-4">
                        <a target="_blank" href={invitation.telegram} className="d-block text-reset text-decoration-none">
                            <Telegram className="text-white" fill="white" width="4rem" height="4rem" />
                            <center>
                                <span>Telegram</span>
                            </center>
                        </a>
                        <a target="_blank" href={invitation.viber} className="d-block text-reset text-decoration-none">
                            <Viber fill="white" width="4rem" height="4rem" />
                            <center>
                                <span>Viber</span>
                            </center>
                        </a>
                    </Card.Body>
                </Card>
            )}
        </div>
    );
};

export default Invitation;
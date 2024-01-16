import { useParams } from "react-router-dom";
import { Button, Card, Spinner } from "react-bootstrap";
import { ReactComponent as Viber } from "../../assets/viber.svg";
import { ReactComponent as Telegram } from "../../assets/telegram.svg";
import { useGetAnnouncerInvitationQuery } from "../api/distributionSlice";
import "./Invitation.scss";

const Invitation: React.FC = () => {
    const {
        providerId,
    } = useParams();

    const {
        isLoading,
        data: invitation,
    } = useGetAnnouncerInvitationQuery(providerId!);

    return (
        <div className="d-flex flex-column align-items-center justify-content-center" style={{ height: "100vh" }}>
            {isLoading && (
                <Spinner />
            )}
            {invitation && (
                <Card bg="primary-subtle" className="invitation-card shadow-sm">
                    <Card.Body className="d-flex gap-4 p-4 text-white">
                        <div>
                            <small className="text-muted">анонсер</small>
                            <h4 className="my-0 text-primary-emphasis">{invitation.announcer.firstName} {invitation.announcer.lastName}</h4>
                        </div>

                        <div className="d-flex gap-1 flex-column">
                            <Button variant="outline-light" target="_blank" href={invitation.links.telegram} className="d-flex gap-2 align-items-center">
                                <Telegram fill="currentColor" width="32px" />
                                <span className="fw-semibold">Telegram</span>
                            </Button>
                            <Button variant="outline-light" target="_blank" href={invitation.links.viber} className="d-flex gap-2 align-items-center">
                                <Viber fill="currentColor" width="32px" />
                                <span className="fw-semibold">Viber</span>
                            </Button>
                        </div>
                    </Card.Body>
                </Card>
            )}
        </div>
    );
};

export default Invitation;
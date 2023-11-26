import React from "react";
import { Card } from "react-bootstrap";
import { ReactComponent as ViberIcon } from "../../assets/viber.svg";

export interface IFeatureProps {
    title: string,
    description: string,
    icon: string,
    implemented: boolean,
};

const Feature: React.FC<IFeatureProps> = props => {
    const {
        title,
        description,
        icon,
        implemented,
    } = props;

    return (
        <Card className="bg-body-tertiary border-0 w-100 rounded-5 shadow">
            <Card.Body className="d-flex gap-3 position-relative flex-column p-4">
                {icon !== "bi-viber" && (
                    <i className={`text-primary bi ${icon} fs-3`}></i>
                )}
                {icon === "bi-viber" && (
                    <ViberIcon className="text-primary align-self-start mt-2" fill="currentColor" width="30px" />
                )}
                <div>
                    <h3 className="fs-5 text-body-emphasis">{title}</h3>
                    <p className="m-0">{description}</p>
                </div>
                {!implemented && (
                    <span className="position-absolute badge rounded bg-info" style={{ top: -5, right: -5 }}>
                        Очікується
                        <span className="visually-hidden">unread messages</span>
                    </span>
                )}
            </Card.Body>
        </Card>
    );
};

export default Feature;
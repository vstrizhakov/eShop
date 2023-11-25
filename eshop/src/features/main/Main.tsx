import React from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Button, Col, Row } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import Feature, { IFeatureProps } from "./Feature";

const Main: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
    } = props;

    const announcerFeatures: IFeatureProps[] = [
        {
            title: "Автоматичне налаштування анонсів",
            description: "Відбір магазинів, комісія та курси валют будуть автоматично розраховуватися під кожного клієнта",
            icon: "bi-wrench-adjustable",
            implemented: true,
        },
        {
            title: "Чат-боти для Telegram та Viber",
            description: "Надайте своїм клієнтам можливість отримувати анонси у потрібних месенджерах за допомогою чат-ботів",
            icon: "bi-robot",
            implemented: true,
        },

        {
            title: "Підписки для клієнтів",
            description: "Списуйте та отримуйте оплату від клієнтів за користування вашими послугами в автоматичному режимі",
            icon: "bi-credit-card",
            implemented: false,
        },
        {
            title: "Статистика",
            description: "Відслідковуйте та аналізуйте як ваші клієнти користуються вашими анонсами",
            icon: "bi-graph-up",
            implemented: false,
        },
        {
            title: "Шаблони анонсів",
            description: "Створюйте власний стиль відображення опису анонсів у ваших клієнтів",
            icon: "bi-card-text",
            implemented: false,
        },
        {
            title: "Колажі",
            description: "Створюйте зображення для анонсів без використання інших застосунків",
            icon: "bi-images",
            implemented: false,
        },
    ];

    const buyerFeatures: IFeatureProps[] = [
        {
            title: "Підтримка Telegram",
            description: "Отримуйте готові анонси одразу у ваших групах або каналах",
            icon: "bi-telegram",
            implemented: true,
        },
        {
            title: "Підтримка Viber",
            description: "Отримуйте готові анонси в особисті повідомлення та пересилайте їх до своїх груп без редагування",
            icon: "bi-viber",
            implemented: true,
        },
        {
            title: "Фільтрування за магазинами",
            description: "Отримуйте анонси тільки з тими магазинами, що вас цікавлять",
            icon: "bi-filter",
            implemented: true,
        },
        {
            title: "Налаштування комісії",
            description: "Вказуйте свою власну комісію та отримуйте анонси з її урахуванням",
            icon: "bi-percent",
            implemented: true,
        },
        {
            title: "Налаштування курсів валют",
            description: "Встановлюйте свої власні курси валют, або користуйтеся курсами НБУ",
            icon: "bi-currency-exchange",
            implemented: true,
        },

        {
            title: "Підтримка каналів у Viber",
            description: "Налаштуйте відправку анонсів напряму у свої канали",
            icon: "bi-megaphone",
            implemented: false,
        },
        {
            title: "Налаштування зображень",
            description: "Керуйте тим, що повинно відображатись за зображеннях",
            icon: "bi-card-image",
            implemented: false,
        },
    ];

    return (
        <>
            {isAuthenticated ? (
                <>
                    <div className="d-flex align-items-center justify-content-center" style={{ height: 240 }}>
                        <LinkContainer to="/addAnnounce">
                            <Button size="lg" className="fw-semibold" variant="outline-primary border-start-0 border-end-0 rounded-0 text-white">
                                ДОДАТИ АНОНС
                            </Button>
                        </LinkContainer>
                    </div>
                </>
            ) : (
                <>
                    <Row className="mb-4">
                        <Col className="py-5">
                            <h1 className="display-5 text-body-emphasis">Створено для анонсерів та байєрів</h1>
                            <p className="lead text-muted">eShop - просте рішення для сфери шопінгу</p>

                            <Button className="fw-semibold">Зареєструватися</Button>
                        </Col>
                    </Row>

                    <div className="mb-5">
                        <h2 className="display-6 text-body-emphasis mb-3">Анонсерам</h2>
                        <Row className="g-3">
                            {announcerFeatures && announcerFeatures.map(feature => (
                                <Col xs={12} md={6} xxl={4}
                                    className="d-flex align-items-stretch">
                                    <Feature
                                        title={feature.title}
                                        description={feature.description}
                                        icon={feature.icon}
                                        implemented={feature.implemented}
                                    />
                                </Col>
                            ))}
                        </Row>
                    </div>

                    <div className="mb-5">
                        <h2 className="display-6 text-body-emphasis mb-3">Байєрам</h2>
                        <Row className="g-3">
                            {buyerFeatures && buyerFeatures.map(feature => (
                                <Col xs={12} md={6} xxl={4}
                                    className="d-flex align-items-stretch">
                                    <Feature
                                        title={feature.title}
                                        description={feature.description}
                                        icon={feature.icon}
                                        implemented={feature.implemented}
                                    />
                                </Col>
                            ))}
                        </Row>
                    </div>
                </>
            )}
        </>
    );
};

export default withAuth(Main);
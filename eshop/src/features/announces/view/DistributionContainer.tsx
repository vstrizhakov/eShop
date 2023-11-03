import React, { useEffect } from "react";
import { useGetDistributionQuery } from "../../api/distributionSlice";
import Distribution from "./Distribution";
import { RootState } from "../../../app/store";
import { ConnectedProps, connect } from "react-redux";
import { setDistribution } from "./viewAnnounceSlice";

const mapStateToProps = (state: RootState) => ({
    distribution: state.viewAnnounce.distribution,
});

const mapDispatchToProps = {
    setDistribution,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

interface IProps extends PropsFromRedux {
    distributionId: string,
};

const DistributionContainer: React.FC<IProps> = props => {
    const {
        distributionId,
        distribution,
        setDistribution,
    } = props;

    const {
        data,
        isLoading,
        isSuccess,
    } = useGetDistributionQuery(distributionId);

    useEffect(() => {
        if (data) {
            setDistribution(data);
        }
    }, [data]);

    if (isLoading) {
        return <>Завантаження...</>
    }

    if (!isSuccess) {
        return <>Під час завантаження сталася помилка</>;
    }

    if (!distribution) {
        return null;
    }

    return <Distribution distribution={distribution} />;
};

export default connector(DistributionContainer);
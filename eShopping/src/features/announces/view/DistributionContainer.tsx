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
        isError,
    } = useGetDistributionQuery(distributionId);

    useEffect(() => {
        if (data && !distribution) {
            setDistribution(data);
        }
    }, [data, distribution, setDistribution]);

    if (isError) {
        return <>Під час завантаження сталася помилка</>;
    }

    if (!distribution) {
        return <>Завантаження...</>
    }

    return <Distribution distribution={distribution} />;
};

export default connector(DistributionContainer);
import React from "react";
import { Composition as CompositionData } from "../api/catalogSlice";
import Distribution from "./Distribution";

interface IProps {
    composition: CompositionData,
};

const Composition: React.FC<IProps> = props => {
    const {
        composition: {
            id: compositionId,
            distributionGroupId,
        },
    } = props;

    return (
        <div>
            Composition #{compositionId}
            {distributionGroupId && (
                <Distribution distributionGroupId={distributionGroupId}/>
            )}
        </div>
    );
};

export default Composition;
import React from "react";
import { Composition, useGetCompositionsQuery } from "../api/catalogSlice";

interface IProps {
    onCompositionSelected: (composition: Composition) => void,
};

const Compositions: React.FC<IProps> = props => {
    const {
        onCompositionSelected,
    } = props;

    const {
        data: compositions,
    } = useGetCompositionsQuery(undefined);

    return (
        <>
            {compositions && compositions.map(composition => (
                <div onClick={() => onCompositionSelected(composition)}>{composition.id}</div>
            ))}
        </>
    );
};

export default Compositions;
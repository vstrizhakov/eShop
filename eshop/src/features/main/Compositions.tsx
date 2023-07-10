import React from "react";
import { useGetCompositionsQuery } from "../api/catalogSlice";

const Compositions: React.FC = () => {
    const {
        data: compositions,
    } = useGetCompositionsQuery(undefined);

    return (
        <>
        {compositions && compositions.map(composition => (
            <div>{composition.id}</div>
        ))}
        </>
    );
};

export default Compositions;
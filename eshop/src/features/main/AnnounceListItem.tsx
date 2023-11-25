import React from "react";
import { Announce } from "../api/catalogSlice";

interface IProps {
    announce: Announce,
};

const AnnounceListItem: React.FC<IProps> = props => {
    const {
        announce: {
            images,
        },
    } = props;

    const mainImage = images[0];
    return (
        <div>
            <img src={mainImage}/>
            
        </div>
    );
};

export default AnnounceListItem;
import React from "react";

const DropdownAnchorToggle = React.forwardRef<HTMLAnchorElement>((props, ref) => (
    <a
        {...props}
        ref={ref}/>
));

export default DropdownAnchorToggle;
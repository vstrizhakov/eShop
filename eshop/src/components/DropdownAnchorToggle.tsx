import React from "react";

const DropdownAnchorToggle = React.forwardRef<HTMLAnchorElement>((props, ref) => (
    // eslint-disable-next-line jsx-a11y/anchor-has-content
    <a
        {...props}
        ref={ref}/>
));

export default DropdownAnchorToggle;
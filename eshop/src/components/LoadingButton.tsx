import React from "react";
import { Button as BootstrapButton, ButtonProps, Spinner } from "react-bootstrap";
import { ReplaceProps, BsPrefixProps } from "react-bootstrap/esm/helpers";

type BsConnectedProps<As extends React.ElementType, P> = React.PropsWithChildren<ReplaceProps<As, BsPrefixProps<As> & P>>;
type BootstrapButtonProps = BsConnectedProps<'button', ButtonProps>;

interface IButtonsProps {
    isLoading?: boolean,
};

const LoadingButton: React.FC<IButtonsProps & BootstrapButtonProps> = props => {
    const {
        isLoading,
        children,
    } = props;

    return (
        <BootstrapButton
            {...(props)}
            disabled={isLoading || props.disabled}>
            {isLoading && (
                <Spinner size="sm" />
            )}
            {!isLoading && children}
        </BootstrapButton>
    );
};

export default LoadingButton;
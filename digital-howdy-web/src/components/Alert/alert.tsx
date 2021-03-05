import React, { useEffect, useState } from 'react';

import './alert.css';

type Props = {
    message: string,
    active: boolean
}

const Alert = (props: Props) => {
    const [active, setActive] = useState(props.active);

    useEffect(() => {
        setActive(props.active);
    }, [props.active]);

    return(
        <div id="alert">
            {active ? 
            <div className="alert">
                {props.message}
            </div>
            : null}
        </div>
    );
}

export const showAlert = () => {

}

export default Alert;
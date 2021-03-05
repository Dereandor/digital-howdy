import React, { useEffect, useState } from 'react';
import { Redirect, Route } from 'react-router-dom';
import { validateToken } from './services/adminService';

const PrivateRoute = ({component, ...rest}: any) => {
    const [validToken, setValidToken] = useState(true);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
        validateUser();
    }, []);

    const validateUser = async () => {
        setLoading(true);
        const valid = await validateToken();
        setLoading(false);
        setValidToken(valid);
    }

    const routeComponent = (props: any) => {
        return (
            loading ? null : 
            validToken ?
                React.createElement(component, props)
                : <Redirect to= {{ pathname: '/login' }} />
        );
    }

    return <Route {...rest} render={routeComponent}/>;
}

export default PrivateRoute;
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import Alert from '../../components/Alert/alert';
import LoginForm from '../../components/LoginForm/loginForm';
import { loginAdmin } from '../../services/adminService';
import { AdminLogin } from '../../types/admin';

const Login = () => {
    const history = useHistory();

    const [error, setError] = useState('');

    const onSubmit = async (adminLogin: AdminLogin) => {
        const loginResponse = await loginAdmin(adminLogin);

        if (loginResponse.error === '') {
            history.push('/dashboard/checkout');
        } else {
            setError(loginResponse.error)
        }
    }

    return(
        <div className="content">
            <Alert message={error} active={error !== ''} />
            <div className="center">
                <div className="container">
                    <p className="heading">Admin Login</p>
                    <LoginForm onSubmit={onSubmit}/>
                </div>
            </div>
            
        </div>
    )
}

export default Login;
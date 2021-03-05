/* import jsSHA from 'jssha'; */
import React, { ChangeEvent, FormEvent, useState } from 'react';
import { AdminLogin } from '../../types/admin';

type Props = {
    onSubmit: (adminLogin: AdminLogin) => void
}

const LoginForm = (props: Props) => {
    const [formData, setFormData] = useState<AdminLogin>({
        username: '',
        hashedPassword: ''
    });

    const onChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, maxLength } = e.target;

        const slicedValue = value.slice(0, maxLength);

        setFormData({
            ...formData,
            [name]: slicedValue
        });
    }

    const onSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        /* var pwdObj = formData.hashedPassword;
        var hashObj = new jsSHA("SHA-512", "TEXT", {numRounds: 1});
        hashObj.update(pwdObj);
        var hash = hashObj.getHash("HEX");
        formData.hashedPassword = hash; */
        props.onSubmit(formData);
    }

    return(
        <form
            className="form"
            onSubmit={onSubmit}
        >
            <label htmlFor="username">Username:</label>
            <input
                type="text"
                id="username"
                name="username"
                required
                maxLength={40}
                onChange={onChange}
                value={formData.username} 
            />
            <label htmlFor="hashedPassword">Password:</label>
            <input
                type="password"
                id="hashedPassword"
                name="hashedPassword"
                required
                maxLength={40}
                onChange={onChange}
                value={formData.hashedPassword} 
            />
            <button type="submit">Submit</button>
        </form>
    )
}

export default LoginForm;
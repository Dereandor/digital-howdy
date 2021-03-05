import React, { ChangeEvent, FormEvent, MouseEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Navbar from '../../components/Navbar/navbar';
import { registerAdmin, getAllAdmins } from '../../services/adminService';
import { AdminRegister, Admin } from '../../types/admin';
import AdminsTable from '../../components/AdminsTable/adminsTable';

const Admins = () => {
    const { t } = useTranslation();

    const [ admin, setAdmin ] = useState<AdminRegister>(
        {
            username: '',
            password: '',
            confirmPassword: ''
        }
    );
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [admins, setAdmins] = useState<Admin[]>([]);

    useEffect(() => {
        getAdmins();
    }, []);

    const getAdmins = async () => {
        const response = await getAllAdmins();

        if(response.data !== null){
            setAdmins(response.data);
        }
    }

    const onAdminDeleted = async () => {
        getAdmins();
    }

    const onAdminAdded = async () => {
        getAdmins();
    }

    const onChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, maxLength } = e.target;

        const slicedValue = value.slice(0, maxLength);

        setAdmin({
            ...admin,
            [name]: slicedValue
        });
    }

    const onInformationClick = (e: MouseEvent<HTMLElement>) => {
        setSuccess('');
    }

    const onSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const response = await registerAdmin(admin);

        if (response.error === '') {
            setError('');
            setSuccess(`User '${admin.username}' ${t('admins.success')}`);
            setAdmin({
                username: '',
                password: '',
                confirmPassword: ''
            });
        } else {
            setError(response.error);
        }
        onAdminAdded();
    }

    const onSortRequested = (key: string, reverse: boolean) => {
        // Have to copy array, else useEffect will not register the change as the pointer is the same
        const adminsCopy: Admin[] = [...admins];
        let sortedAdmins: Admin[];

        // Only reverse list if this was the last sorted column
        if (reverse) {
            sortedAdmins = adminsCopy.reverse();
        } else {
            switch (key) {
                case ("Username"):
                    sortedAdmins = adminsCopy.sort((a, b) => a.username.localeCompare(b.username));
                    break;
                default:
                    sortedAdmins = adminsCopy;
                    break;
            }
        }
        setAdmins(sortedAdmins);
    }

    return(
        <div className="dashboard">
            <Navbar />
            <main>
                <div>
                    <h1>{t('admins.header')}</h1>
                    <hr></hr>
                    {true ?
                        <p className="info-message" value-hidden={success === '' ? 'true' : 'false'} onClick={onInformationClick}>
                        {success}
                        <div className="close-button">
                            <i className="fas fa-times"></i>
                        </div>
                    </p>
                    : null}
                    
                    <form
                        className="dashboard-form"
                        autoComplete="off"
                        onSubmit={onSubmit}>
                        <label htmlFor="username">{t('admins.username')}</label><br />
                        <input
                            type="text"
                            id="username"
                            name="username"
                            onChange={onChange}
                            value={admin.username}
                            maxLength={30}
                            required
                        /><br />
                        <label htmlFor="password">{t('admins.password')}</label><br />
                        <input
                            type="password"
                            id="password"
                            name="password"
                            onChange={onChange}
                            value={admin.password}
                            maxLength={50}
                            required
                        /><br />
                        <label htmlFor="confirmPassword">{t('admins.confirm-password')}</label><br />
                        <input
                            type="password"
                            id="confirmPassword"
                            name="confirmPassword"
                            onChange={onChange}
                            value={admin.confirmPassword}
                            maxLength={50}
                            required
                        /><br />
                        <p className="error-message" hidden={error === ''}>{error}</p>
                        <button id="submitCheckout" type="submit">{t('admins.submit')}</button>
                    </form>
                    <AdminsTable admins={admins} onSortRequested={onSortRequested} onAdminDeleted={onAdminDeleted} />
                </div>
            </main>
        </div>
    );
}

export default Admins;
import React, { FormEvent, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { getVisitorByPhone } from '../../services/visitorsService';
import { registerVisit } from '../../services/visitsService';
import { useTranslation } from 'react-i18next';

import './registration.css';

import Employee from '../../types/employee';
import FormData from '../../types/formData';

import RegistrationForm from '../../components/RegistrationForm/registrationForm';
import Alert from '../../components/Alert/alert';
import Visitor from '../../types/visitor';
import { GetAllEmployeeNameAndId } from '../../services/employeesService';

const Registration = () => {
    const history = useHistory();

    const { t } = useTranslation();

    const [employees, setEmployees] = useState<Employee[]>([
        
    ]);

    const [error, setError] = useState<string>('');

    useEffect(() => {
        getEmployees();
    }, []);

    const getEmployees = async () => {
        const response = await GetAllEmployeeNameAndId();

        if (response.data !== null) {
            setEmployees(response.data);
        } else {
            setError(response.error);
        }
    }

    const onFormSubmit = async (e: FormEvent<HTMLFormElement>, formData: FormData) => {
        const result = await registerVisit(formData);

        if (result.data !== null) {
            history.push('/printing');
        } else {
            setError(result.error ?? '');
        }       
    }

    const tryGetVisitor = async (phone: string, callback: (visitor: Visitor) => void) => {
        const visitor = await getVisitorByPhone(phone);
        
        if (visitor.data !== null) {
            callback(visitor.data);
        }
    }    

    return(
        <div className="content">
            <Alert message={error} active={error !== ''} />
            <div className="center">
                <div className="container">
                    <p className="heading">{t('registration.please')}</p>
                    <RegistrationForm 
                        onSubmit={onFormSubmit}
                        onPhoneChange={tryGetVisitor}
                        employees={employees}
                    />
                </div>
            </div>
        </div>
    )
}

export default Registration;
import React, { FormEvent, useEffect, useState } from "react";
import Employee from "../../types/employee";
import AutoComplete from "../Autocomplete/autocomplete";

import './registrationForm.css';

import FormData from '../../types/formData';
import Alert from "../Alert/alert";
import Visitor from "../../types/visitor";
import { useTranslation } from "react-i18next";

export type Props = {
    onSubmit: (e: FormEvent<HTMLFormElement>, formData: FormData) => void,
    onPhoneChange: (phone: string, callback: (visitor: Visitor) => void) => any,
    employees: Employee[],
}

const RegistrationForm = (props: Props) => {
    const [formData, setFormData] = useState<FormData>({
        phone: '',
        name: '',
        organization: '',
        employee: undefined
    });

    const [error, setError] = useState('');

    const { t } = useTranslation();

    useEffect(() => {
    }, [formData]);

    const validateForm = (): boolean => {
        const validateEmployee = (formData.employee !== undefined && props.employees.find(e => e.name === formData.employee?.name) !== undefined);

        if (!validateEmployee) {
            setError('Employee not found.');
        }

        return (validateEmployee);
    }

    const onEmployeeSelect = (name: string) => {
        const employee = props.employees.find(e => e.name === name);
        setFormData({ ...formData, employee: employee});
    }

    const onVisitorFound = (visitor: Visitor) => {
        setFormData({
            ...formData,
            phone: visitor.phone,
            name: visitor.name,
            organization: visitor.organization.name
        });
    }

    const onPhoneChange = async (e: any) => {
        const phone = e.currentTarget.value;
        await props.onPhoneChange(phone, onVisitorFound);
    }

    const isValidPhoneNumber = (phone: string) => {
        let valid: boolean = !isNaN(+phone.substring(phone.indexOf('+') + 1));
        valid = valid && (phone.indexOf('+') <= 0);
        return valid;
    }

    const onSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const valid = validateForm();

        if (valid)
        {
            props.onSubmit(e, formData);
        }
    }

    const onChange = (e: any) => {
        const { name, value, maxLength } = e.target;

        if (name === 'phone' && (!isValidPhoneNumber(value))) {
            return;
        } 

        const slicedValue = value.slice(0, maxLength);

        setFormData({
            ...formData,
            [name]: slicedValue
        });
    }

    return(
        <div>
            <form
                data-testid="registration-form"
                className="form"
                autoComplete="off"
                onSubmit={onSubmit}>
                <label htmlFor="phone">{t('registration.phone')}</label><br />
                <div className="after"><input
                    data-testid="phone"
                    type="text"
                    id="phone"
                    name="phone"
                    required
                    maxLength={11}
                    onChange={(e) => { onChange(e); onPhoneChange(e)}}
                    value={formData.phone}
                /><br /></div>
                
                <label htmlFor="name">{t('registration.name')}</label><br />
                <input
                    data-testid="name"
                    type="text" 
                    id="name" 
                    name="name" 
                    required
                    maxLength={40}
                    onChange={onChange}
                    value={formData.name}
                /><br />
                <label htmlFor="organization">{t('registration.organization')}</label><br />
                <input
                    data-testid="organization"
                    type="text"
                    id="organization"
                    name="organization"
                    required
                    maxLength={40}
                    onChange={onChange}
                    value={formData.organization}
                /><br />
                <label htmlFor="employee">{t('registration.employee')}</label><br />
                <AutoComplete name="employee"
                                placeholder={t('registration.autocomplete')}
                                maxLength={40} 
                                suggestions={props.employees.map(emp => emp.name)} 
                                onSelect={onEmployeeSelect} 
                                dataTestId="employee"
                                boolean={false}/>
                                
                <Alert message={error} active={error !== ''} />
                <p className="disclaimer">
                    {t('registration.disclaimer')}
                </p>
                <button data-testid="submit" type="submit">{t('registration.button')}</button>
            </form>
        </div>
    )
}

export default RegistrationForm;
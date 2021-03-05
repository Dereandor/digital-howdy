import React, { FormEvent, KeyboardEvent, MouseEvent, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Navbar from '../../components/Navbar/navbar';
import { getAllVisits, updateEndDateVisit } from '../../services/visitsService';
import { VisitDetails } from '../../types/visit';
import VisitUpdate from '../../types/visitUpdate';

import './checkout.css';

const Checkout = () => {
    const { t } = useTranslation();
    const [visit, setVisit] = useState<VisitDetails | null>(null);
    const [query, setQuery] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const onQueryEnter = async () => {
        if (query === '') {
            return;
        }
        
        const response = await getAllVisits(query, true, true);

        if (response.error === '') {
            if (response.data?.length !== 0) {
                setVisit(response.data ? response.data[0] : null);
                setSuccess('');
                setError('');
            } else {
                setError(t('checkout.no-visit'));
            }
            
        } else {
            setError(response.error);
        }
    }

    const onQueryKeyDown = (event: KeyboardEvent<HTMLInputElement>) => {
        if (event.keyCode === 13) {
            onQueryEnter();
        }
    }

    const onSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (visit !== null) {
            const id = visit.id ?? -1;
            const visitUpdate: VisitUpdate = {
                id: id,
                endDate: new Date(Date.now())
            }
            const response = await updateEndDateVisit(visitUpdate);
            if (response.error === '') {
                setSuccess(`${visit.visitor.name} ${t('checkout.success')}`)
                setQuery('');
                setVisit(null);
                setError('');
            } else {
                setError(response.error);
            }
        }
    }

    const onInformationClick = (e: MouseEvent<HTMLElement>) => {
        setSuccess('');
    }

    return(
        <div className="dashboard">
            <Navbar />
            <main>
                <div>
                    <h1>{t('checkout.header')}</h1>
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
                        <label htmlFor="query">{t('checkout.phone')}</label><br />
                        <input
                            type="text"
                            id="query"
                            name="query"
                            onKeyDown={onQueryKeyDown}
                            onChange={(e) => setQuery(e.currentTarget.value)}
                            value={query}
                            required
                        /><br />
                        <p className="error-message" hidden={error === ''}>{error}</p>
                        <label htmlFor="name">{t('checkout.name')}</label><br />
                        <input 
                            type="text"
                            id="name" 
                            name="name" 
                            value={visit?.visitor.name ?? ''}
                            readOnly 
                        /><br />
                        <label htmlFor="organization">{t('checkout.organization')}</label><br />
                        <input
                            type="text"
                            id="organization"
                            name="organization"
                            value={visit?.visitor.organization.name ?? ''}
                            required
                            readOnly
                        /><br />
                        <label htmlFor="checkIn">{t('checkout.check-in')}</label><br />
                        <input
                            type="text"
                            id="checkIn"
                            name="checkIn"
                            value={visit?.startDate !== undefined ? new Date(visit?.startDate ?? '').toLocaleString() : ''}
                            required
                            readOnly
                        /><br />
                        <button id="submitCheckout" type="submit" disabled={visit === null}>{t('checkout.button')}</button>
                    </form>
                </div>
                
            </main>
        </div>
    )
}

export default Checkout;
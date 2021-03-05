import React, { FormEvent, useEffect, useState, ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import Navbar from '../../components/Navbar/navbar';
import VisitsTable from '../../components/VisitsTable/visitsTable';
import { getAllVisits } from '../../services/visitsService';
import { VisitDetails } from '../../types/visit';

import './visitorLog.css';

const VisitorLog = () => {
    const { t } = useTranslation();

    const [search, setSearch] = useState('');
    const [visits, setVisits] = useState<VisitDetails[]>([]);
    const [currentOnly, setCurrentOnly] = useState(false);
    

    useEffect(() => {
        getVisits(search, currentOnly);
    }, [search, currentOnly]);

    useEffect(() => {

    }, [visits])

    const getVisits = async (query: string, check: boolean) => {
        const response = await getAllVisits(query, check);

        if (response.data !== null) {
            setVisits(response.data);
        }
    }

    const checkboxChange = (e: ChangeEvent<HTMLInputElement>) => {
        const checked = e.currentTarget.checked;
        setCurrentOnly(checked);
    }

    const onSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        getVisits(search, currentOnly);
    }

    const onSortRequested = (key: string, reverse: boolean) => {
        // Have to copy array, else useEffect will not register the change as the pointer is the same
        const visitsCopy: VisitDetails[] = [...visits];
        let sortedVisits: VisitDetails[];

        // Only reverse list if this was the last sorted column
        if (reverse) {
            sortedVisits = visitsCopy.reverse();
        } else {
            switch (key) {
                case ("reference"):
                    sortedVisits = visitsCopy.sort((a, b) => a.reference - b.reference);
                    break;
                case ("startDate"):
                    sortedVisits = visitsCopy.sort((a, b) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime());
                    break;
                case ("endDate"):
                    sortedVisits = visitsCopy.sort((a, b) => new Date(a.endDate).getTime() - new Date(b.endDate).getTime());
                    break;
                case ("name"):
                    sortedVisits = visitsCopy.sort((a, b) => a.visitor.name.localeCompare(b.visitor.name));
                    break;
                case ("phone"):
                    sortedVisits = visitsCopy.sort((a, b) => a.visitor.phone.localeCompare(b.visitor.phone));
                    break;
                case ("organization"):
                    sortedVisits = visitsCopy.sort((a, b) => a.visitor.organization.name.localeCompare(b.visitor.organization.name));
                    break;
                case ("employee"):
                    sortedVisits = visitsCopy.sort((a, b) => a.employee.name.localeCompare(b.employee.name));
                    break;
                default:
                    sortedVisits = visitsCopy;
                    break;
            }
        }

        setVisits(sortedVisits);
    }

    return(
        <div className="dashboard">
            <Navbar />
            <main>
                <div>
                    <h1>{t('dashboard.header')}</h1>
                    <hr/>
                    <form className="dashboard-form" onSubmit={onSubmit}>
                        <input 
                            type="text" 
                            placeholder={t('dashboard.search')}
                            onChange={(e) => setSearch(e.currentTarget.value)}
                        />
                        <button hidden type="submit">{t('dashboard.go')}</button>
                    </form>
                    <label className="checkbox">
                    <input type="checkbox"
                        className="currentStatus"
                        checked={currentOnly}
                        onChange={checkboxChange}
                    />
                    Show only current visitors
                    <span className="checkmark"></span>
                    </label>
                    <VisitsTable visits={visits} onSortRequested={onSortRequested} />
                </div>
                
            </main>
        </div>
    )
}

export default VisitorLog;
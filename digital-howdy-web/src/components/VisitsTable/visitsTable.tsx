import React, { MouseEvent, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { VisitDetails } from '../../types/visit';

import './visitsTable.css';

type Props = {
    visits: VisitDetails[]
    onSortRequested: (key: string, reverse: boolean) => void
}

const VisitsTable = (props: Props) => {
    const { t } = useTranslation();

    const [previousColumnTarget, setPreviousColumnTarget] = useState<EventTarget & HTMLTableHeaderCellElement>();

    const onHeaderClick = (e: MouseEvent<HTMLTableHeaderCellElement>) => {
        const previousKey = previousColumnTarget?.getAttribute("value-key") ?? '';
        setPreviousColumnTarget(e.currentTarget);
        const key = e.currentTarget.getAttribute("value-key") ?? '';
        const valueSort = e.currentTarget.getAttribute("value-sort") ?? '';
        const reverse = (key === previousKey);
        deselectPreviousColumn();
        e.currentTarget.setAttribute("value-sort", valueSort === "up" ? "down" : "up");
        props.onSortRequested(key, reverse);
    }

    const deselectPreviousColumn = () => {
        previousColumnTarget?.setAttribute('value-sort', '');
    }

    return(
        <div>
            <table className="overview-table">
                <tr>
                    <th onClick={onHeaderClick} value-key="reference">
                        {t('dashboard.reference')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="startDate">
                        {t('dashboard.check-in')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="endDate">
                        {t('dashboard.check-out')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="name">
                        {t('dashboard.name')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="phone">
                        {t('dashboard.phone')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="organization">{
                        t('dashboard.organization')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="employee">
                        {t('dashboard.employee')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                </tr>
                {props.visits.map((visit, index) => {
                    return(
                        <tr key={index}>
                            <td>
                                {visit.reference}
                            </td>
                            <td>
                                {new Date(visit.startDate).toLocaleString()}
                            </td>
                            <td>
                                {new Date(visit.endDate).getFullYear() > 1900 ? new Date(visit.endDate).toLocaleString() : t('dashboard.currently-in')}
                            </td>
                            <td>
                                {visit.visitor.name}
                            </td>
                            <td>
                                {visit.visitor.phone}
                            </td>
                            <td>
                                {visit.visitor.organization.name}
                            </td>
                            <td>
                                {visit.employee.name}
                            </td>
                        </tr>
                    )
                })}
            </table>
        </div>
    )
}

export default VisitsTable;
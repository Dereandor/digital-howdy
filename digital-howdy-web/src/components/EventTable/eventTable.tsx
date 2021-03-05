import React, { MouseEvent, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { EventLogEntry } from '../../types/events';

type Props = {
    events: EventLogEntry[],
    onSortRequested: (key: string, reverse: boolean) => void
}

const EventTable = (props: Props) => {
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

    return (
        <div>
            <table className="overview-table">
                <tr>
                    <th onClick={onHeaderClick} value-key="date">
                        {t('event-log.date')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                    <th onClick={onHeaderClick} value-key="description">
                        {t('event-log.description')}
                        <div className="sort-arrow">
                            <i className=" sort-arrow-up-down fas fa-sort"></i>
                            <i className=" sort-arrow-up fas fa-sort-up"></i>
                            <i className=" sort-arrow-down fas fa-sort-down"></i>
                        </div>
                    </th>
                </tr>
                {props.events.map((event, index) => {
                    return(
                        <tr key={index}>
                            <td>
                                {new Date(event.date).toLocaleString()}
                            </td>
                            <td>
                                {event.description}
                            </td>
                        </tr>
                    )
                })}
            </table>
        </div>
    );
}

export default EventTable;
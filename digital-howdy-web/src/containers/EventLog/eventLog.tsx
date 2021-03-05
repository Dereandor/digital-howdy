import React, { FormEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import EventTable from '../../components/EventTable/eventTable';
import Navbar from '../../components/Navbar/navbar';
import { getAllEvents } from '../../services/eventLogService';
import { EventLogEntry } from '../../types/events';

const EventLog = () => {
    const { t } = useTranslation();

    const [search, setSearch] = useState('');
    const [events, setEvents] = useState<EventLogEntry[]>([]);

    useEffect(() => {
        getEvents('');
    }, []);

    const getEvents = async (query: string) => {
        const response = await getAllEvents(query);
        if (response.data !== null) {
            setEvents(response.data);
        }
    }

    const onSortRequested = (key: string, reverse: boolean) => {
        // Have to copy array, else useEffect will not register the change as the pointer is the same
        const eventsCopy: EventLogEntry[] = [...events];
        let sortedEvents: EventLogEntry[];

        // Only reverse list if this was the last sorted column
        if (reverse) {
            sortedEvents = eventsCopy.reverse();
        } else {
            switch (key) {
                case ("date"):
                    sortedEvents = eventsCopy.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
                    break;
                case ("description"):
                    sortedEvents = eventsCopy.sort((a, b) => a.description.localeCompare(b.description));
                    break;
                default:
                    sortedEvents = eventsCopy;
                    break;
            }
        }

        setEvents(sortedEvents);
    }

    const onSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        getEvents(search);
    }

    return (
        <div className="dashboard">
            <Navbar />
            <main>
                <div>
                    <h1>{t('event-log.header')}</h1>
                    <hr/>
                    <form className="dashboard-form" onSubmit={onSubmit}>
                        <input
                            type="text"
                            placeholder={t('event-log.search')}
                            onChange={(e) => setSearch(e.currentTarget.value)}
                        />
                        <button type="submit">{t('event-log.go')}</button>
                    </form>
                    <EventTable events={events} onSortRequested={onSortRequested} />
                </div>
            </main>
        </div>
    );
}

export default EventLog;
export type EventLogEntry = {
    id: number,
    date: Date,
    description: string
}

export type EventLogsResponse = {
    data: EventLogEntry[] | null,
    error: ''
}
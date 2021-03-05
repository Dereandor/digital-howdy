import axios from 'axios';
import * as config from '../config/config.json';
import { EventLogsResponse } from '../types/events';

const url = config.apiBaseUrl;

export const getAllEvents = async (query: string): Promise<EventLogsResponse> => {
    try {
        const result = await axios.get(`${url}/eventlogs?query=${query}`);
        const response: EventLogsResponse = {
            data: result.data,
            error: ''
        }

        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response: EventLogsResponse = {
            data: null,
            error: error
        }
        return response;
    }
}
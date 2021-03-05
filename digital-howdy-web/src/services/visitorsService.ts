import axios from 'axios';
import { VisitorResult } from '../types/visitor';
import * as config from '../config/config.json';

const url = config.apiBaseUrl;

export const getVisitorByPhone = async (phone: string): Promise<VisitorResult> => {
    try {
        // TODO: Move apiBaseUrl to config
        const result = await axios.get(`${url}/visitors?phone=${phone}`);
        const response = {
            data: result.data,
            error: ''
        };

        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response: VisitorResult = {
            data: null,
            error: error
        }
        return response;
    }
}
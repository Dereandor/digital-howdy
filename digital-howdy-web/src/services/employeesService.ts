import axios from 'axios';
import { EmployeeNamesResponse } from '../types/employee';
import * as config from '../config/config.json';

const url = config.apiBaseUrl;

export const GetEmployeeById = async (id: number) => {
    try {
        const result = await axios.get(`${url}/employees/${id}`)
        const data = result.data;
        return data;
    } catch (e) {
        return null;
    }
}

export const GetAllEmployeeNameAndId = async (): Promise<EmployeeNamesResponse> => {
    try {
        const result = await axios.get(`${url}/employees/names`);
        const response = {
            data: result.data,
            error: ''
        }
        return response;
    } catch (e) {
        const error = e.response?.data.message ?? 'Unable to contact database.';

        const response: EmployeeNamesResponse = {
            data: null,
            error: error
        }
        return response;
    }
}
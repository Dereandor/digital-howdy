import Employee from "./employee";
import Visitor from "./visitor";

type Visit = {
    id: number,
    visitor_id: number,
    employee_id: number,
    start_date: string,
    end_date: string,
    reference: number
}

export type VisitDetails = {
    id: number,
    visitor: Visitor,
    employee: Employee,
    startDate: Date,
    endDate: Date,
    reference: number
}

export type VisitsResponse = {
    data: VisitDetails[] | null,
    error: string
}

export type VisitUpdateResponse = {
    error: string
}

export default Visit;
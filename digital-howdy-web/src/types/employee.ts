type Employee = {
    id: number,
    name: string
};

export type EmployeeNamesResponse = {
    data: Employee[] | null,
    error: string
}

export default Employee;
import Employee from "./employee";

type FormData = {
    phone: string,
    name: string,
    organization: string,
    employee: Employee | undefined
};

export default FormData;
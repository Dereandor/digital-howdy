type Visitor = {
    id: number,
    name: string,
    phone: string,
    organization: Organization
}

type Organization = {
    id: number,
    name: string
}

export type VisitorResult = {
    data: Visitor | null,
    error: string
}

export default Visitor;
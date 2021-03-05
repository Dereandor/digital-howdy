export type RegistrationResponse = {
    data: RegistrationConfirmation | null,
    error: string
}

export type RegistrationConfirmation = {
    id: number,
    reference: number
}
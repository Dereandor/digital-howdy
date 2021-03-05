export type AdminLogin = {
    username: string,
    hashedPassword: string
}

export type Admin = {
    id: number,
    username: string,
    token: string
}

export type AdminResult = {
    data: Admin[] | null,
    error: string
}

export type LoginResponse = {
    data: Admin | null,
    error: string
}

export type AdminRegister = {
    username: string,
    password: string,
    confirmPassword: string
}

export type RegisterResponse = {
    data: Admin | null,
    error: string
}
import React, { FormEvent } from 'react';
import { render, fireEvent, waitForElement } from '@testing-library/react';
import { act } from 'react-dom/test-utils';

import RegistrationForm, {Props} from '../../components/RegistrationForm/registrationForm';

import FormData from '../../types/formData';
import Visitor from '../../types/visitor';

function renderRegistrationForm(props: Partial<Props> = {}) {
    const defaultProps: Props = {
        onSubmit(e: FormEvent<HTMLFormElement>, formData: FormData): void {
            return;
        },
        onPhoneChange(phone: string, callback: (visitor: Visitor) => void): void {
            const visitor: Visitor = { id: 3, phone: '23456789', name: 'Nikola Tesla', organization: { id: 1, name: 'Tesla' }};

            if (phone === visitor.phone) {
                callback(visitor);
            }
        },
        employees: []
    };
    return render(<RegistrationForm {...defaultProps} {...props}/>);
}

describe("<RegistrationForm />", () => {
    test("Should display a blank registration form.", async () => {
        const { getByTestId } = renderRegistrationForm();

        const form = await getByTestId("registration-form");

        expect(form).toHaveFormValues({
            phone: '',
            name: '',
            organization: '',
            employee: ''
        });
    })

    test("Should allow entering a phone number", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = await getByTestId("phone");

        fireEvent.change(phone, { target: { value: '12345678' }});

        expect(phone).toHaveValue('12345678');
    });

    test("Should not allow phone longer than 11", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = await getByTestId("phone");

        fireEvent.change(phone, { target: { value: '123456789101112' }});

        expect(phone).toHaveValue('12345678910');
    });

    test("Should allow phone with country code", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = await getByTestId("phone");

        fireEvent.change(phone, { target: { value: '+4712345678' }});

        expect(phone).toHaveValue('+4712345678');
    });

    test("Should allow phone with invalid country code", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = await getByTestId("phone");

        fireEvent.change(phone, { target: { value: '-fgfrsjh4712345678' }});

        expect(phone).toHaveValue('');
    });

    test("Should set form data when phone match", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = getByTestId("phone");

        fireEvent.change(phone, { target: { value: '23456789' }});
        
        const name = await waitForElement(() => getByTestId("name"));

        expect(phone).toHaveValue('23456789');
        expect(name).toHaveValue('Nikola Tesla');
    });

    test("Should not set form data when no phone match", async () => {
        const { getByTestId } = renderRegistrationForm();
        const phone = getByTestId("phone");

        fireEvent.change(phone, { target: { value: '12345678' }});
        
        const name = await waitForElement(() => getByTestId("name"));

        expect(phone).toHaveValue('12345678');
        expect(name).toHaveValue('');
    });

    test("Should allow entering a name", async () => {
        const { getByTestId } = renderRegistrationForm();
        const name = await getByTestId("name");

        act(() => {
            fireEvent.change(name, { target: { value: 'Bob Bobsson' }});
        });

        expect(name).toHaveValue('Bob Bobsson');
    });

    test("Should not allow name longer than 40", async () => {
        const { getByTestId } = renderRegistrationForm();
        const name = await getByTestId("name");

        fireEvent.change(name, { target: { value: 'Bob Bobssonforewjhgfojfpojfgpojfijijojoirej' }});

        expect(name).toHaveValue('Bob Bobssonforewjhgfojfpojfgpojfijijojoi');
    });

    test("Should allow entering organization", async () => {
        const { getByTestId } = renderRegistrationForm();
        const organization = await getByTestId("organization");

        fireEvent.change(organization, { target: { value: 'NTNU' }});

        expect(organization).toHaveValue('NTNU');
    });

    test("Should not allow organization longer than 40", async () => {
        const { getByTestId } = renderRegistrationForm();
        const organization = await getByTestId("organization");

        fireEvent.change(organization, { target: { value: 'Bob Bobssonforewjhgfojfpojfgpojfijijojoirej' }});

        expect(organization).toHaveValue('Bob Bobssonforewjhgfojfpojfgpojfijijojoi');
    });

    test("Should allow entering an employee", async () => {
        const { getByTestId } = renderRegistrationForm();
        const employee = await getByTestId("employee");

        act(() => {
            fireEvent.change(employee, { target: { value: 'Bob Bobsson' }});
        });

        expect(employee).toHaveValue('Bob Bobsson');
    });

    test("Should not allow employee name longer than 40", async () => {
        const { getByTestId } = renderRegistrationForm();
        const employee = await getByTestId("employee");

        fireEvent.change(employee, { target: { value: 'Bob Bobssonforewjhgfojfpojfgpojfijijojoirej' }});

        expect(employee).toHaveValue('Bob Bobssonforewjhgfojfpojfgpojfijijojoi');
    });
})
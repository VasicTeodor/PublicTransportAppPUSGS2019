import { Address } from './address';
import { Ticket } from './ticket';

export interface User {
    id: number;
    name: string;
    surname: string;
    userName: string;
    email: string;
    dateOfBirth: Date;
    address: Address;
    userType: string;
    active: string;
    documentUrl: string;
    tickets?: Ticket[];
    userRoles?: string[];
}

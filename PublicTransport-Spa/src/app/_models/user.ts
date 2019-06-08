import { Address } from './address';
import { Ticket } from './ticket';

export interface User {
    id?: number;
    name?: string;
    surname?: string;
    userName?: string;
    email?: string;
    dateOfBirth?: Date;
    address?: Address;
    userType?: string;
    accountStatus?: string;
    active?: string;
    documentUrl?: string;
    publicId?: string;
    tickets?: Ticket[];
    userRoles?: string[];
}

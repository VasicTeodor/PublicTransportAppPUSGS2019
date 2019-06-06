import { Location } from './location';

export interface Bus {
    id: number;
    location: Location;
    busNumber: number;
    inUse: boolean;
}

import { Address } from './address';
import { Location } from './location';
import { StationLine } from './stationLine';

export interface Station {
    id: number;
    name: string;
    address: Address;
    location: Location;
    stationLines: StationLine[];
}

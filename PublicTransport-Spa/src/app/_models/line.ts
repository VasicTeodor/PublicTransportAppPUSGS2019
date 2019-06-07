import { Bus } from './bus';
import { StationLine } from './stationLine';

export interface Line {
    id: number;
    lineNumber: number;
    name: string;
    stations: StationLine[];
    buses: Bus[];
}

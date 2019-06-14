import { Line } from './line';

export interface TimeTable {
    id: number;
    type: string;
    day: string;
    line: Line;
    departures: string;
    lineId: number;
}

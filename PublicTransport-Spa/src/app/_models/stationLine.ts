import { Line } from './line';
import { Station } from './station';

export interface StationLine {
    lineId: number;
    stationId: number;
    line: Line;
    station: Station;
}

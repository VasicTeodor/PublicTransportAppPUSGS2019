import { Item } from './item';
import { Pricelist } from './pricelist';

export interface PricelistItem {
    id: number;
    item: Item;
    pricelist: Pricelist;
    price: number;
}

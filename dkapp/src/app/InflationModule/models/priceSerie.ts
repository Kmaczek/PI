import * as moment from 'moment';
import { Moment } from 'moment';

export interface IPriceSerie{
    price: number;
    createdDate: string;
}

export class PriceSerie implements IPriceSerie {

    constructor(
        price, 
        created) {
        this.price = price;
        this.createdDate = created;
        
    }

    public price: number;
    public createdDate: string;
    public createdMoment(): Moment
    {
        return moment(this.createdDate);
    }
}
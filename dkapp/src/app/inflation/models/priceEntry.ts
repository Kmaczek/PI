
import { Moment } from 'moment';
import moment from 'moment';


export interface IPriceEntry {
  price: number;
  createdDate: string;
}

export class PriceEntry implements IPriceEntry {
  public price: number;
  public createdDate: string;

  constructor(price, created) {
    this.price = price;
    this.createdDate = created;
  }

  public createdDateAsMoment(): Moment {
    return moment(this.createdDate);
  }
}

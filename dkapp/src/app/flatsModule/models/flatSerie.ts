export class FlatSerie {

    constructor(
        id, 
        day, 
        avgPricePerMeter, 
        avgPrice, 
        amount) {
        this.id = id;
        this.day = day;
        this.avgPricePerMeter = avgPricePerMeter;
        this.avgPrice = avgPrice;
        this.amount = amount;
    }

    public id: number;
    public day: Date;
    public avgPricePerMeter: number;
    public avgPrice: number;
    public amount: number;
}
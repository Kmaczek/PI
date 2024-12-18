export class Product {

    constructor(init?: Partial<Product>) {
        Object.assign(this, init);
    }

    public id: number = 0;
    public name: string = '';
    public code: string = '';
    public uri: string = '';

    public parserTypeId: number = 0;
    public params: string = '';
    public track: boolean = false;
    public createdDate: Date = new Date();
    public updatedDate: Date = new Date();
    public activeFrom: Date = new Date();
    public activeTo: Date = new Date();
    public category: number = 0;
    public latestPriceDetailId?: number;
}
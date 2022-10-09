export class Product {

    constructor(init?: Partial<Product>) {
        Object.assign(this, init);
    }

    public id: number;
    public name: string;
    public code: string;
    public uri: string;
}
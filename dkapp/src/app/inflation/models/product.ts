export class Product {

    constructor(init?: Partial<Product>) {
        Object.assign(this, init);
    }

    public id: number = 0;
    public name: string = '';
    public code: string = '';
    public uri: string = '';
}
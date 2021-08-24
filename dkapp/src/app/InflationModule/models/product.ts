export class Product {

    constructor(
        id, 
        name, 
        code) {
        this.id = id;
        this.name = name;
        this.code = code;
    }

    public id: number;
    public name: string;
    public code: string;
}
export class Product {

    constructor(
        id, 
        name, 
        code,
        site) {
        this.id = id;
        this.name = name;
        this.code = code;
        this.site = site;
    }

    public id: number;
    public name: string;
    public code: string;
    public site: number;
}
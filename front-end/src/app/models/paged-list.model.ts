export class PagedList<T> {
    public page: number;
    public pageSize: number;
    public totalCount: number;
    public items: T[];

    constructor(page: number, pageSize: number, totalCount: number) {
        this.page = page;
        this.pageSize = pageSize;
        this.totalCount = totalCount;
        this.items = [];
    }
}

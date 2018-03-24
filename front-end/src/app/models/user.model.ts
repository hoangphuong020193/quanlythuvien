export class User {
    public userId: number;
    public userName: string;
    public displayName: string;
    public accessToken: string;
    public isLoggedOut: boolean;
    public firstName: string;
    public middleName: string;
    public lastName: string;

    constructor(userId: number = 0, userName: string = '') {
        this.userId = userId;
        this.userName = userName;
    }
}

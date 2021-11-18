export class UserInfo {
    public email: string = "";
    public password: string = "";
};

export class UserRegister {
    public email: string = "";
    public password: string = "";
    public name:string="";
};

export class Token {
    public AcessToken: string = "";
}

export class Package {
    public httpCode: number = 0;
    public httpStatus: string = "";
    public status: string = "";
    public data: any = {};
    public notifications: any ={}
}

export class ApiConfig {
    public privateKey: string = "";
    public publicKey: string = "";
}
   
export class Comics {
    public resourceURI: string = "";
    public name: string = "";
}
   

   
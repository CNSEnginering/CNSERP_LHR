import * as moment from "moment";

export interface ICurrencyRateHistoryDto {
    audtdate: moment.Moment | undefined;
    audtuser: string | undefined;
    curname: string | undefined;
    symbol: string | undefined;
    ratedate: moment.Moment | undefined;
    currate: number | undefined;
    curid: string | undefined;
    cmpid: string | undefined;
    id: string | undefined;
}

export class CurrencyRateHistoryDto implements ICurrencyRateHistoryDto {
    audtdate: moment.Moment | undefined;
    audtuser: string | undefined;
    curname: string | undefined;
    symbol: string | undefined;
    ratedate: moment.Moment | undefined;
    currate: number | undefined;
    curid: string | undefined;
    cmpid: string | undefined;
    id: string | undefined;

    constructor(data?: ICurrencyRateHistoryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.audtdate = data["audtdate"]
                ? moment(data["audtdate"].toString())
                : <any>undefined;
            this.audtuser = data["audtuser"];
            this.curname = data["curname"];
            this.symbol = data["symbol"];
            this.ratedate = data["ratedate"]
                ? moment(data["audtdate"].toString())
                : <any>undefined;
            this.currate = data["currate"];
            this.curid = data["curid"];
            this.cmpid = data["cmpid"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): CurrencyRateHistoryDto {
        data = typeof data === "object" ? data : {};
        let result = new CurrencyRateHistoryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === "object" ? data : {};
        data["audtdate"] = this.audtdate
            ? moment(data["audtdate"].toString())
            : <any>undefined;
        data["audtuser"] = this.audtuser;
        data["curname"] = this.curname;
        data["symbol"] = this.symbol;
        data["ratedate"] = this.ratedate
            ? moment(data["audtdate"].toString())
            : <any>undefined;
        data["currate"] = this.currate;
        data["curid"] = this.curid;
        data["cmpid"] = this.cmpid;
        data["id"] = this.id;
        return data;
    }
}

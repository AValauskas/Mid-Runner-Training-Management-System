export interface IRecords{     
    records:IRecordsByPlace[];
   
  }

  export interface IRecordsByPlace{     
    distance:string;
    time: string;
    place: string;
    competition: string;
    dateString:string;
    date:Date;
  }
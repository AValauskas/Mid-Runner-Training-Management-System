export class ITrainingTemplate{
    description:string;
    trainingtype:string;
    destinition:number;    
    repeats:number;  
    personal:boolean;
    toDisplay:string;
    sets:Sets[];
    id:string;
  }
  interface Sets{
    distance:number;
    pace:number;
    rest:number;
    }
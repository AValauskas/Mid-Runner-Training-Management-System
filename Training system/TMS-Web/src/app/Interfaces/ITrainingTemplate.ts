export interface ITrainingTemplate{
    description:string;
    trainingtype:string;
    destinition:number;    
    repeats:number;  
    personal:boolean;
    sets:Sets[];
  }
  interface Sets{
    distance:number;
    pace:number;
    rest:number;
    }
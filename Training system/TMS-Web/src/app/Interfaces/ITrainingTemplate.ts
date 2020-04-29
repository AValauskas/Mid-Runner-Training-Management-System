export class ITrainingTemplate{
    description:string;
    trainingType:string;
    destinition:number;    
    repeats:number;  
    isPersonal:boolean;
    toDisplay:string;
    sets:Sets[];
    owner:string;
    id:string;
  }
  interface Sets{
    distance:number;
    pace:number;
    rest:number;
    }
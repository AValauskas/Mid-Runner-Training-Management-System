export class IPersonalTrainingFew{
    day:Date;
    trainTemplateId:string;
    athleteIds:string[];   
    coachId:string;    
    athleteReport:string;    
    description:string;     
    place:string;  
    id:string;    
    sets:Sets[];
    
  }

  interface Sets{
  distance:number;
  pace:number;
  rest:number;
  }
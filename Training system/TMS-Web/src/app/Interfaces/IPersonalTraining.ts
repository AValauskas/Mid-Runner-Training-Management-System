export interface IPersonalTraining{
    day:Date;
    trainTemplateId:string;
    athleteId:string;   
    coachId:string;    
    athleteReport:string;    
    description:string;     
    place:string;  
    id:string;    
    sets:Sets[];
    definition:string;
    results:Sets[];
  }

  interface Sets{
  distance:number;
  pace:number;
  rest:number;
  }
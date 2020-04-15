import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-coach-training-assign-modal',
  templateUrl: './coach-training-assign-modal.component.html',
  styleUrls: ['./coach-training-assign-modal.component.scss']
})
export class CoachTrainingAssignModalComponent implements OnInit {

  @Input() dateClicked :string;
  canFillForm:boolean;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges() {
     
    var dateTraining=new Date(this.dateClicked);
    console.log( this.canFillForm);
    console.log (dateTraining.getTime());
    console.log (Date.now());
    if(dateTraining.getTime() >= Date.now() )
    {
      this.canFillForm = true;
          
    }
    else{
      this.canFillForm = false;
    }
  
  }
}

import { Component, OnInit ,Type} from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient ,HttpHeaders } from '@angular/common/http';
import {GlobalConstants } from '../global-constants';
import {Package} from '../global-models';
import { ApiConfig } from '../global-models'; 
import { Comics } from '../global-models'; 
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';


var errorMessage :string = "";
var errorTitle :string = "";
var errorCardTitle :string = "";

@Component({
  selector: 'ngbd-modal-confirm-autofocus',
  template: `
  <div class="modal-header">
    <h4 class="modal-title" id="modal-title">{{errorC}}</h4>
  </div>
  <div class="modal-body">
    <p><strong>{{errorT}}</strong></p>
    <p style="white-space: pre-line" > {{errorM}}</p>
  </div>
  <div class="modal-footer">
    <button type="button" ngbAutofocus class="btn btn-danger" (click)="modal.close('Ok click')">Ok</button>
  </div>
  `
})

export class NgbdModalConfirmAutofocus {
  constructor(public modal: NgbActiveModal) {}

  public errorM:string = errorMessage;
  public errorT:string = errorTitle;
  public errorC:string = errorCardTitle;


}

const MODALS: {[name: string]: Type<any>} = {
  autofocus: NgbdModalConfirmAutofocus
};

@Component({
  selector: 'app-home-screen',
  templateUrl: './home-screen.component.html',
  styleUrls: ['./home-screen.component.css'],
})


export class HomeScreenComponent implements OnInit {

  constructor(private http: HttpClient,private router: Router,private _modalService: NgbModal) { };
  public nots:any[]=["teste","teste2"];

  ngOnInit(): void {

    this.token =  localStorage.getItem('userToken');

    this.http.get<any>(`${GlobalConstants.apiURL}/login/v1/validate/${this.token}`).subscribe(p => {
      this.options = {
        headers: new HttpHeaders(
          {
            'Content-Type': 'application/json',
            'Authorization': this.token 
          })
      };
    },err=>{
      this.pack = err.error;
      this.router.navigate(['/redirect']); 
    });


    
  }

  private token :any;
  public ErrorMessage: string = "";
  public ShowModal: boolean = false;
  public ShowHome: boolean = true;
  public ShowConfig: boolean = false;
  public ShowSeries: boolean = false;
  public ShowAbout: boolean = false;
  public ComicName: string = "";
  public HeroName: string = "";
  public ComicNameFixed: string = "";
  public  listComics: any = [];
  public  listHeroes: any = [];

  private pack:Package = {
    httpCode:0,
    httpStatus:"",
    status:"",
    data:{},
    notifications:{}
  };

  public apiConfig:ApiConfig = {
    privateKey : "",
    publicKey:""
  };

  private options : any = {};
  Modal(event:boolean) {
    this.ShowModal = event;
  }

  Comics() {
    this.ShowHome = true;
    this.ShowConfig = false;
    this.ShowSeries = false;
    this.ShowAbout = false;

  }

  Series() {
    this.ShowSeries = true;
    this.ShowConfig = false;
    this.ShowHome = false;
    this.ShowAbout = false;

  }

  About() {
    this.ShowAbout = true;
    this.ShowSeries = false;
    this.ShowConfig = false;
    this.ShowHome = false;
  }

  SubmitComicSearch(){

    this.http.get<Package>(`${GlobalConstants.apiURL}/marvel/v1/series/${this.ComicName}`,<Object>this.options).subscribe(p => {
      this.pack = p;
      this.ComicNameFixed = this.ComicName;
      this.listComics = this.pack.data.results;

    },err=>{
      this.BuildErrorMessage(err.error,"Error") ;   
    });

  }

  
  SubmitCharacterSearch(){

    this.http.get<Package>(`${GlobalConstants.apiURL}/marvel/v1/hero/${this.HeroName}`,<Object>this.options).subscribe(p => {
      this.pack = p;

      this.listHeroes = this.pack.data.results;

    },err=>{
      this.BuildErrorMessage(err.error,"Error") ;   
    });

  }

  Config() {

    this.http.get<Package>(`${GlobalConstants.apiURL}/marvel/v1/config`,<Object>this.options).subscribe(p => {
      this.apiConfig = p.data;
    },err=>{
      this.apiConfig.privateKey = "";
      this.apiConfig.publicKey = "";
    });
    this.ShowConfig = true;
    this.ShowHome = false;
    this.ShowSeries = false;
    this.ShowAbout = false;
  }

  LogOut() {
    localStorage.setItem('userToken', "");
    this.router.navigate(['/redirect']); 
  }
  
  open(name: string) {
    this._modalService.open(MODALS[name]);
  }

  BuildErrorMessage(arrayErrors:any,title:string) {
    
    errorTitle = this.pack.httpStatus;
    errorCardTitle = title;
    errorMessage = "";
    
    arrayErrors.notifications.forEach((key : any, val: any) => {
      errorMessage += `${key.message}\n`
    });

    this.open("autofocus");
  }

  BuildSuccessMessage(message:string,title:string) {
    errorTitle = this.pack.httpStatus;
    errorCardTitle = title;
    errorMessage = message;
    this.open("autofocus");
  }


  SubmitApiConfig()
  {

    this.http.post<Package>(`${GlobalConstants.apiURL}/marvel/v1/new/config`,this.apiConfig,<Object>this.options).subscribe(p => {

      this.BuildSuccessMessage("Sucessful Operation","New config was saved");
      
    },err=>{
      this.BuildErrorMessage(err.error,"ApiCondfig Failed") ;   
    });
  }




}




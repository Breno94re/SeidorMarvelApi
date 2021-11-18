import { Component, OnInit,Type} from '@angular/core';
import {sha512} from 'js-sha512';
import { HttpClient } from '@angular/common/http';
import{ GlobalConstants } from '../global-constants';
import{ UserInfo} from '../global-models';
import{ Token} from '../global-models';
import{ Package} from '../global-models';
import{ UserRegister} from '../global-models';
import { Router } from '@angular/router';
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
  selector: 'app-login-screen',
  templateUrl: './login-screen.component.html',
  styleUrls: ['./login-screen.component.css']
})
export class LoginScreenComponent implements OnInit {

  title = 'marvelseidor';

  ngOnInit(): void {
    
  }
  constructor(private http: HttpClient,private router: Router,private _modalService: NgbModal) { };
  public email: string = "";
  public password: string = "";
  public singUpEmail: string = "";
  public singPassword: string = "";
  public singUpName: string = "";
  public ErrorMessage: string = "";
  public Signupclick:boolean = false;
  public ShowModal: boolean = false;



  private userRegister:UserRegister = {
    email: "",
    password: "",
    name:""
  };

  private userInfo:UserInfo = {
  email: "",
  password: "",
  };

  private token:Token = {
    AcessToken: ""
  };

  private pack:Package = {
    httpCode:0,
    httpStatus:"",
    status:"",
    data:{},
    notifications:{}
  };
  
  ChangeScreen(event:boolean) {
    this.Signupclick = event;
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


  open(name: string) {
    this._modalService.open(MODALS[name]);
  }

  LogIn() {

    this.userInfo.email = this.email;

    this.userInfo.password = sha512(this.password);


    this.http.post<Package>(`${GlobalConstants.apiURL}/login/v1`, this.userInfo ).subscribe(p => {
      this.pack = p;
      localStorage.setItem('userToken', this.pack.data.token);
      this.router.navigate(['/home']); 

    },err=>{
      this.BuildErrorMessage(err.error,"Log-in Failed") ;
    });
    

  }

  SignUp() {

    this.userRegister.email = this.singUpEmail;

    this.userRegister.password = sha512(this.singPassword);

    this.userRegister.name = this.singUpName;

    this.http.post<Package>(`${GlobalConstants.apiURL}/login/v1/new/user`, this.userRegister ).subscribe(p => {
      this.BuildSuccessMessage("Sucessful Registration","New Account");
    },err=>{
      this.BuildErrorMessage(err.error,"Registration Failed") ;
    });
    

  }


}

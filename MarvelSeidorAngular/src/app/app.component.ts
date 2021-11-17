import { Component } from '@angular/core';
import {sha512} from 'js-sha512';
import { HttpClient } from '@angular/common/http';
import{ GlobalConstants } from './global-constants';
import{ UserInfo} from './global-models';
import{ Token} from './global-models';
import{ Package} from './global-models';
import{ UserRegister} from './global-models';
import { Router } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  
})

export class AppComponent {
  title = 'marvelseidor';

  constructor(private http: HttpClient,private router: Router) { }
  

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

  Modal(event:boolean) {
    this.ShowModal = event;
  }



  LogIn() {

    this.userInfo.email = this.email;

    this.userInfo.password = sha512(this.password);


    this.http.post<Package>(`${GlobalConstants.apiURL}/login/v1`, this.userInfo ).subscribe(p => {
      this.pack = p;

      localStorage.setItem('userToken', this.pack.data.token);

      this.router.navigate(['/home']); 

    },err=>{
      this.pack = err.error;
      this.ErrorMessage = `${this.pack.httpStatus}`
      this.Modal(true);
    });
    

  }

  SignUp() {

    this.userRegister.email = this.singUpEmail;

    this.userRegister.password = sha512(this.singPassword);

    this.userRegister.name = this.singUpName;


    this.http.post<Package>(`${GlobalConstants.apiURL}/login/v1/new/user`, this.userRegister ).subscribe(p => {
      this.pack = p;
      this.ErrorMessage = `${this.pack.status}`
      this.Modal(true);
    },err=>{
      this.pack = err.error;
      this.ErrorMessage = `${this.pack.notifications[0].message}`
      this.Modal(true);
    });
    

  }


}



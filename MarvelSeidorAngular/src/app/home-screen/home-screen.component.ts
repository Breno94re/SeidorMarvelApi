import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home-screen',
  templateUrl: './home-screen.component.html',
  styleUrls: ['./home-screen.component.css']
})
export class HomeScreenComponent implements OnInit {

 

  ngOnInit(): void {
  }

  constructor(private http: HttpClient,private router: Router) { }

  private token  = localStorage.getItem('userToken');





}

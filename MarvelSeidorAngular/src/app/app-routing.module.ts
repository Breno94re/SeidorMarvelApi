import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeScreenComponent} from './home-screen/home-screen.component';
import { LoginScreenComponent} from './login-screen/login-screen.component';

const routes: Routes = [ { path: '', redirectTo: 'home', pathMatch: 'full' },
{ path: 'redirect', redirectTo: 'login', pathMatch: 'full' },
{ path: 'home', component:HomeScreenComponent } ,
{ path: 'login', component:LoginScreenComponent }  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

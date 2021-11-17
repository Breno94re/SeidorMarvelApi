import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeScreenComponent} from './home-screen/home-screen.component';

const routes: Routes = [ { path: '', redirectTo: '', pathMatch: 'full' },
{ path: 'home-screen', component:HomeScreenComponent }  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

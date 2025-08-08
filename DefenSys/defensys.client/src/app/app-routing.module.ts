import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// 1. First, import the component "destinations".
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ScannersComponent } from './pages/scanners/scanners.component';

// 2. Now, define the routes in the "address book".
const routes: Routes = [
  // This is a special rule for the root path. If the user navigates to the
  // empty path (e.g., 'http://localhost:4200/'), they will be automatically
  // redirected to the '/dashboard' path.
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },

  // Route definition for the dashboard page.
  // When the URL path is '/dashboard', Angular will render the DashboardComponent.
  { path: 'dashboard', component: DashboardComponent },

  // Route definition for the scanners page.
  // When the URL path is '/scanners', Angular will render the ScannersComponent.
  { path: 'scanners', component: ScannersComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

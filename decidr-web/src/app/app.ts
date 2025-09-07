import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,           
  imports: [RouterOutlet, CommonModule, MatToolbarModule, RouterModule],   
  templateUrl: './app.html',
  styleUrls: ['./app.scss'] 
})
export class App {
  protected readonly title = 'Decidr';
}

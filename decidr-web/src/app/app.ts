import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-root',
  standalone: true,           
  imports: [RouterOutlet, CommonModule, MatToolbarModule],   
  templateUrl: './app.html',
  styleUrls: ['./app.scss'] 
})
export class App {
  protected readonly title = 'Decidr';
}

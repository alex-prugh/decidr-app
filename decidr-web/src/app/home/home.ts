import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';
import { Set } from '../shared/models/set';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class HomeComponent implements OnInit {
  sets: Set[] = [];
  loading = true;
  error = '';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.loadSets();
  }

  loadSets() {
    this.loading = true;
    this.http.get<Set[]>('https://localhost:5001/api/sets')
      .subscribe({
        next: (res) => {
          this.sets = res;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load sets';
          this.loading = false;
          console.error(err);
        }
      });
  }

  createPopularMoviesSet(): void {
    // Make a POST request to the API
    this.http.get<Set>('https://localhost:5001/api/movies/popular', {})
      .subscribe({
        next: (newSet) => {
          // On success, navigate to the new set's detail page
          this.router.navigate(['/sets', newSet.id]);
        },
        error: (err) => {
          console.error('Error creating popular movies set:', err);
          alert('Failed to get popular movies.');
        }
      });
  }
}

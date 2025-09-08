// src/app/set-detail/set-detail.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Set } from '../shared/models/set';
import { Card } from '../shared/models/card';

@Component({
  selector: 'app-set-detail',
  templateUrl: './set-detail.html',
  styleUrls: ['./set-detail.scss'],
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
  ]
})
export class SetDetailComponent implements OnInit {
  set: Set | undefined;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const setId = params.get('id');
      if (setId) {
        this.getSetData(setId);
      }
    });
  }

  getSetData(id: string): void {
    const url = `https://localhost:5001/api/sets/${id}`;
    this.http.get<Set>(url).subscribe(
      (data) => {
        this.set = data;
      },
      (error) => {
        console.error('Error fetching set data:', error);
        alert('An error occurred. Redirecting to the home page.'); // Display a simple alert
        this.router.navigate(['/home']); // Redirect to the home page route ('/')
      }
    );
  }

  likeCard(card: Card): void {
    const url = `https://localhost:5001/api/cards/${card.id}/like`;
    this.http.post<boolean>(url, {}).subscribe({
      next: (success) => {
        if (success) {
          card.isLiked = true;
          card.isDisliked = false;
        }
      },
      error: (err) => {
        console.error('Error liking card:', err);
      }
    });
  }

  dislikeCard(card: Card): void {
    const url = `https://localhost:5001/api/cards/${card.id}/dislike`;
    this.http.post<boolean>(url, {}).subscribe({
      next: (success) => {
        if (success) {
          card.isLiked = false;
          card.isDisliked = true;
        }
      },
      error: (err) => {
        console.error('Error disliking card:', err);
      }
    });
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }
}
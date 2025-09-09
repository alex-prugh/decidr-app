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
  currentCardIndex = 0;

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
        alert('An error occurred. Redirecting to the home page.');
        this.router.navigate(['/home']);
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
          // After a vote, go to the next card
          this.nextCard();
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
          // After a vote, go to the next card
          this.nextCard();
        }
      },
      error: (err) => {
        console.error('Error disliking card:', err);
      }
    });
  }

  nextCard(): void {
    if (this.set && this.currentCardIndex < this.set.cards.length - 1) {
      this.currentCardIndex++;
    }
  }

  previousCard(): void {
    if (this.currentCardIndex > 0) {
      this.currentCardIndex--;
    }
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }
}
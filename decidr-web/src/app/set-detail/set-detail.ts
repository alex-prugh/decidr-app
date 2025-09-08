// src/app/set-detail/set-detail.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Set } from '../shared/models/set';

@Component({
  selector: 'app-set-detail',
  templateUrl: './set-detail.html',
  styleUrls: ['./set-detail.scss'],
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

  likeCard(cardId: number): void {
    console.log(`Liked card with id: ${cardId}`);
    // Implement API call for liking a card here
  }

  dislikeCard(cardId: number): void {
    console.log(`Disliked card with id: ${cardId}`);
    // Implement API call for disliking a card here
  }

  goHome(): void {
    this.router.navigate(['/home']);
  }
}
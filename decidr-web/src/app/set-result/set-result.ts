// src/app/set-result/set-result.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { SetResult } from '../shared/models/set-result';

@Component({
  selector: 'app-set-result',
  standalone: true,
  imports: [RouterModule, CommonModule, ],
  templateUrl: './set-result.html',
  styleUrls: ['./set-result.scss']
})
export class SetResultComponent implements OnInit {
  setResult: SetResult | undefined;
  loading = true;
  error = '';
  
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const setId = params.get('id');
      if (setId) {
        this.getSetResults(setId);
      }
    });
  }

  getSetResults(id: string): void {
    const url = `https://localhost:5001/api/sets/${id}/results`;
    this.http.get<SetResult>(url)
      .subscribe({
        next: (res) => {
          this.setResult = res;
          this.loading = false;
        },
        error: (err) => {
          console.error('Error fetching set results:', err);
          this.error = 'Failed to load set results.';
          this.loading = false;
        }
    });
  }
}
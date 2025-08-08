import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-status-checker',
  templateUrl: './status-checker.component.html',
  styleUrls: ['./status-checker.component.scss']
})
export class StatusCheckerComponent {
  public apiStatus?: string;
  public errorMessage?: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  checkApiStatus() {
    this.errorMessage = undefined;
    this.apiStatus = 'Loading...';

    this.http.get<any>(this.baseUrl + 'api/status').subscribe({
      next: (result) => {
        // پیام را به صورت خوانا نمایش می‌دهیم
        this.apiStatus = `${result.status} (at ${new Date(result.timestamp).toLocaleTimeString()})`;
      },
      error: (error) => {
        this.apiStatus = undefined;
        this.errorMessage = 'Failed to connect to the API. Is the backend running?';
        console.error(error);
      }
    });
  }
}

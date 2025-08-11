// scanners.component.ts

import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

// (Optional but good practice) Define an interface for the scan result
// to match our backend's ScanResult DTO.
export interface ScanResult {
  isVulnerable: boolean;
  message: string;
  testedUrl?: string;
}

@Component({
  selector: 'app-scanners',
  templateUrl: './scanners.component.html',
  styleUrls: ['./scanners.component.scss']
})
export class ScannersComponent {

  public selectedScanner: string | null = null;

  // New properties for our form and results
  public targetUrl: string = ''; // This will hold the URL from the input box
  public isLoading: boolean = false; // To show a loading indicator during the scan
  public scanResult: ScanResult | null = null; // To store the result from the API

  // We need to inject HttpClient to make API calls.
  constructor(private http: HttpClient) { }

  selectScanner(scannerName: string) {
    this.selectedScanner = scannerName;
    // Reset previous results when a new scanner is selected
    this.scanResult = null;
    this.targetUrl = '';
  }

  deselectScanner() {
    this.selectedScanner = null;
  }

  // This is our new, more generic scan function.
  // It takes the API endpoint as a parameter.
  performScan(apiEndpoint: string) {
    if (!this.targetUrl) {
      alert('Please enter a target URL.');
      return;
    }

    this.isLoading = true;
    this.scanResult = null;

    const requestBody = { url: this.targetUrl };

    this.http.post<ScanResult>(apiEndpoint, requestBody)
      .subscribe({
        next: (result) => {
          this.scanResult = result;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('API call failed:', err);
          this.scanResult = {
            isVulnerable: false,
            message: 'An error occurred while communicating with the API.'
          };
          this.isLoading = false;
        }
      });
  }
}


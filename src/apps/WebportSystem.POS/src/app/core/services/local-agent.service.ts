import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root',
})
export class LocalAgentService {
  private agentUrl = 'http://localhost:5055';

  constructor(private http: HttpClient) {}

  ping(): Observable<string> {
    return this.http.get<string>(`${this.agentUrl}/ping`);
  }

  ping2(): Observable<{ status: string; version: string }> {
    return this.http.get<{ status: string; version: string }>(`${this.agentUrl}/ping`);
  }

  print(text: string) {
    return this.http.post(`${this.agentUrl}/print`, { text });
  }
}

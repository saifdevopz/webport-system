import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../../shared/components/header/header';
import { Footer } from '../../shared/components/footer/footer';
import { LocalAgentService } from '../../core/services/local-agent.service';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, Header, Footer],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
  host: {
    class: 'min-h-screen flex flex-col',
  },
})
export class MainLayout {
  constructor(private agent: LocalAgentService) {}

  ngOnInit() {
    console.log('STARTED');
    this.agent.ping().subscribe({
      next: (res) => console.log('Agent response:', res),
      error: (err) => console.error('Agent not reachable:', err),
    });
  }
}

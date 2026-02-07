import { Component } from '@angular/core';
import { Github, LucideAngularModule, Twitter } from 'lucide-angular';

@Component({
  selector: 'app-footer',
  imports: [LucideAngularModule],
  templateUrl: './footer.html',
})
export class Footer {
  protected readonly icons = { Github, Twitter };
}

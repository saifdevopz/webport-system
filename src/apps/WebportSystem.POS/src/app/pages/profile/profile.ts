import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { LucideAngularModule, Mail, Phone, User, MapPin } from 'lucide-angular';
import { profileFeature } from './store/profile-feature';
import { toSignal } from '@angular/core/rxjs-interop';

import { profileActions } from './store/profile-actions';
import { authFeatures } from '../../shared/store/auth-feature';
import { Storage } from '../../core/services/storage';

@Component({
  selector: 'app-profile',
  imports: [LucideAngularModule],
  templateUrl: './profile.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Profile implements OnInit {
  protected readonly icons = { Mail, Phone, User, MapPin };
  private readonly store = inject(Store);
  private readonly storage = inject(Storage);
  protected readonly profile = toSignal(this.store.select(profileFeature.selectProfile));
  protected readonly loading = toSignal(this.store.select(profileFeature.selectLoading));
  protected readonly userId = toSignal(this.store.select(authFeatures.selectUserId));

  ngOnInit(): void {
    const userId = this.userId() || this.storage.getUserId();
    console.log('User ID:', userId);
    if (userId) {
      this.store.dispatch(profileActions.load({ userId }));
    }
  }
}

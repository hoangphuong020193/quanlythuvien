import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LsHelper } from '../../shareds/helpers/ls.helper';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';

@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  templateUrl: 'app.component.html'
})

export class AppComponent implements OnInit {
  constructor(
    private store: Store<fromRoot.State>) { }

  public ngOnInit(): void {
    this.store.select(fromRoot.getCurrentUser).subscribe((user) => {
      if (user) {
        if (user.isLoggedOut) {
          LsHelper.clearStorage();
        } else {
          LsHelper.save(LsHelper.UserStorage, user);
        }
      }
    });
  }
}

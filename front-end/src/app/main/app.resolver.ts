import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class DataResolver implements Resolve<any> {
  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): any {
    return Observable.of({ res: 'I am data' });
  }
}

// an array of services to resolve routes with data
export const APP_RESOLVER_PROVIDERS: any[] = [
  DataResolver
];

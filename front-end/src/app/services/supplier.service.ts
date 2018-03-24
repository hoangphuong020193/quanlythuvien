import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import { Supplier } from '../models/supplier.model';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as supplierAction from '../store/actions/supplier.action';

@Injectable()
export class SupplierService {
    private apiURL: string = Config.getPath(PathController.Supplier);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getListSupplier(): Observable<Supplier[]> {
        return this.http.get(this.apiURL + '/ReturnListSupplier').pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new supplierAction.FetchSupplier(res));
                    return res as Supplier[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public saveSupplier(supplier: Supplier): Observable<number> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/SaveSupplier/',
            supplier, { headers }).pipe(
                tap(
                    (res: any) => {
                        supplier.id = res;
                        this.store.dispatch(new supplierAction.SaveSupplier(supplier));
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }
}

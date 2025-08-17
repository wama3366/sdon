import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Customer } from './customer.dto';

@Injectable({ providedIn: 'root' })
export class CustomerService {
    constructor(private http: HttpClient) {
    }

    getAllCustomers(): Observable<Customer[]> {
        const headers = new HttpHeaders({
            'X-CSRF': '1'
        });

        return this.http.get<Customer[]>('/remote/customers', {headers});
    }
}
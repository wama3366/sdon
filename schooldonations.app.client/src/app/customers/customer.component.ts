import { Component, OnInit } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';

import { CustomerService } from './customer.service';
import { Customer } from './customer.dto';

@Component({
    selector: 'app-customers',
    templateUrl: './customer.component.html',
    imports: [NgIf, NgFor],
    standalone: true,
})

export class CustomerComponent implements OnInit {
    customers: Customer[] = [];
    dataLoaded = false;

    constructor(private data: CustomerService) {
    }

    ngOnInit() {
        this.data.getAllCustomers().subscribe(x => {
            this.customers = x, this.dataLoaded = true;
        });
    }
}

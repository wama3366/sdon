export interface Customer {
    firstName: string;
    lastName: string;

    billingAddressLine1: string;
    billingAddressLine2: string;
    billingCity: string;
    billingState: string;
    billingZipCode: string;
    billingCountry: string;

    shippingAddressLine1: string;
    shippingAddressLine2: string;
    shippingCity: string;
    shippingState: string;
    shippingZipCode: string;
    shippingCountry: string;

    // Meta
    rowVersion: number;
}
export interface AuthUser {
    sub: string;
    'bff:logout_url': string;

    name?: string;
    email?: string;

    // make this indexable.
    [key: string]: string | undefined;
}
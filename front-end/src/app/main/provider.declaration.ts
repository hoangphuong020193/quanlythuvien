import { ErrorHandler } from '@angular/core';

/* Http Services */
import { Http, RequestOptions } from '@angular/http';
import { JwtModule } from '@auth0/angular-jwt';

/* Guards */
import { HomeGuard } from '../guards/home.guard';

/* Handlers */
import { SystemErrorHandler, ResponseHandler } from '../shareds/helpers';
import { LsHelper } from '../shareds/helpers';

/* Services */
import { RouterService } from '../services/router.service';

export const services: any = [
    UserService,
    RouterService,
    LibraryService,
    {
        provide: ErrorHandler,
        useClass: SystemErrorHandler
    },
    ResponseHandler
];

export const guards: any = [
    HomeGuard,
];

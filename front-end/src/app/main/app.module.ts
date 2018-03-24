declare var ENV: string;
import * as $ from 'jquery';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  PreloadAllModules,
  RouterModule
} from '@angular/router';
import { StoreRouterConnectingModule } from '@ngrx/router-store';

import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { BootstrapModalModule } from 'angularx-bootstrap-modal';

import { NgxPaginationModule } from 'ngx-pagination';
import { AppComponent } from '../components/app/app.component';
import { environment } from '../../environments/environment';
import { AppState, InternalStateType } from '../services/app.service';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { ROUTES } from './app.routes';

import '../../styles/main.scss';
import { reducers } from '../store/reducers';
import * as components from './component.declaration';
import * as providers from './provider.declaration';
import { SafeUrlPipe } from '../shareds/pipe/safe-url.pipe';

import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { QuillModule } from 'ngx-quill';
import { JwtModule } from '@auth0/angular-jwt';
import { LsHelper } from '../shareds/helpers/ls.helper';
import { NgxCarouselModule } from 'ngx-carousel';
import 'hammerjs';
import { NgxQRCodeModule } from 'ngx-qrcode2';

const APP_PROVIDERS: any[] = [
  ...APP_RESOLVER_PROVIDERS,
  ...providers.services,
  ...providers.guards,
  AppState,
];

const APP_COMPONENTS: any[] = [
  ...components.CommonComponents,
  ...components.Components
];

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    SafeUrlPipe,
    ...APP_COMPONENTS
  ],
  imports: [ // import Angular's modules
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => LsHelper.getAccessToken(),
        whitelistedDomains: ['localhost:5000', 'localhost:3000']
      }
    }),
    StoreModule.forRoot(reducers),
    RouterModule.forRoot(ROUTES, { useHash: false, preloadingStrategy: PreloadAllModules }),
    StoreRouterConnectingModule,
    'production' !== ENV ? StoreDevtoolsModule.instrument({ maxAge: 50 }) : [],
    BootstrapModalModule,
    NgxPaginationModule,
    QuillModule,
    ToastModule.forRoot(),
    NgxCarouselModule,
    NgxQRCodeModule,
  ],
  providers: [ // expose our Services and Providers into Angular's dependency injection
    environment.ENV_PROVIDERS,
    APP_PROVIDERS
  ],
  entryComponents: [

  ]
})

export class AppModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { MaterialModule } from './modules/material.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './layout/footer/footer.component';
import { HeaderComponent } from './layout/header/header.component';
import { ContentComponent } from './layout/content/content.component';
import { CommentComponent } from './components/comment/comment.component';
import { PageComponent } from './components/page/page.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SnackbarComponent } from './components/common/snackbar/snackbar.component';
import { ReplaceLineBreaksPipe } from './pipes/replace-line-breaks.pipe';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    ContentComponent,
    CommentComponent,
    PageComponent,
    SnackbarComponent,
    ReplaceLineBreaksPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    SnackbarComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing'

import { CommentComponent } from './comment.component';
import { ReplaceLineBreaksPipe } from 'src/app/pipes/replace-line-breaks.pipe';
import { SnackbarComponent } from '../common/snackbar/snackbar.component';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { FormBuilder } from '@angular/forms';

describe('CommentComponent', () => {
  let component: CommentComponent;
  let fixture: ComponentFixture<CommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations:
        [
          CommentComponent,
        ],
      imports: [HttpClientTestingModule, MatSnackBarModule],
      providers: [ReplaceLineBreaksPipe, SnackbarComponent, MatSnackBar, FormBuilder]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('form should be invalid', () => {
    component.commentForm.controls['text'].setValue('');
    component.commentForm.controls['username'].setValue('');
    expect(component.commentForm.valid).toBeFalsy();
  });

  it('form should be valid', () => {
    component.commentForm.controls['text'].setValue('test text');
    component.commentForm.controls['username'].setValue('test user');
    expect(component.commentForm.valid).toBeTruthy();
  });

});

import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { CommentService } from 'src/app/services/comment.service';
import { Comment } from 'src/app/models/comment.model';
import { HttpRequestResult } from 'src/app/models/http-request-result.model';
import { SnackbarComponent } from 'src/app/components/common/snackbar/snackbar.component';
import { MessageType } from 'src/app/models/enums/enums';
import { ErrorHandleHelper } from 'src/app/infrastructure/helpers/error-handle.helper';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit {

  @ViewChild('search') searchElement!: ElementRef;
  @ViewChild(FormGroupDirective) formDirective!: FormGroupDirective;

  public commentForm: FormGroup;
  public commentItems: any;
  public commentCount: number;

  currentComment: any = null;

  constructor(
    private commentService: CommentService,
    public snackbar: SnackbarComponent,
    private formBuilder: FormBuilder,
    private scroller: ViewportScroller
  ) {
    this.commentCount = 0;
    this.commentForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      text: ['', [Validators.required]],
    });
  }

  scrollFn(anchor: string): void {
    this.scroller.scrollToAnchor(anchor)
  }

  getErrorMessage(element: string): string {
    return this.commentForm.controls[element].hasError('required') ?
      'The ' + element + ' is required!' :
      '';
  }

  ngOnInit(): void {
    this.LoadComment();
    this.LoadCommentCount();
  }

  LoadComment() {
    this.commentService.getAllComment().subscribe(
      (result: HttpRequestResult<Comment[]>) => {
        if (result.isFailed) {
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        }
        else {
          if (result.value != null) {
            this.commentItems = result.value;
            //console.log(result.value);
          } else {
            this.snackbar.openSnackBar('problem!', MessageType.Error);
          }
        }
      },
      (error: HttpErrorResponse) => {
        return ErrorHandleHelper.handleError(error, this.snackbar);
      }
    );
  }

  LoadCommentCount() {
    this.commentService.getCountComment().subscribe(
      (result: HttpRequestResult<number>) => {
        if (result.isFailed) {
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        }
        else {
          if (result.value != null) {
            this.commentCount = result.value;
          } else {
            //console.log('problem!');
            this.snackbar.openSnackBar('problem!', MessageType.Error);
          }
        }
      },
      (error: HttpErrorResponse) => {
        return ErrorHandleHelper.handleError(error, this.snackbar);
      }
    );
  }

  public trackByFn = (index: number, item: any) => item;

  replay(item: any) {
    this.currentComment = item;
    setTimeout(() => {
      this.searchElement.nativeElement.focus();
    }, 0);
  }

  removeReplay() {
    this.currentComment = null;
  }

  register(data: any): void {
    const model = {
      "Username": data.username,
      "Text": data.text,
      "Level": (this.currentComment != null) ? this.currentComment.level : 0,
      "ParentId": (this.currentComment != null) ? this.currentComment.id : null,
    };

    this.commentService.register(model).subscribe(
      (result: HttpRequestResult<number>) => {
        if (result.isFailed) {
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        } else {
          if (result.value != null && result.value > 0) {
            this.LoadComment();
            this.LoadCommentCount();
            if ((this.currentComment != null)) {
              this.scrollFn('table-' + this.currentComment.id);
            } else {
              this.scrollFn('div-count');
            }

            this.commentForm.reset();
            this.formDirective.resetForm();

            this.removeReplay();
            this.snackbar.openSnackBar(result.successes, MessageType.Success);
          } else {
            this.snackbar.openSnackBar('problem!', MessageType.Error);
          }
        }
      },
      (error: HttpErrorResponse) => {
        return ErrorHandleHelper.handleError(error, this.snackbar);
      }
    );
  }


}

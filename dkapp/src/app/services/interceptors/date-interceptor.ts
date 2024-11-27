import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DateInterceptor implements HttpInterceptor {
  public intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      map((event: HttpEvent<unknown>) => {
        if (event instanceof HttpResponse) {
          this.postProcessDates(event.body);
          return event;
        }
      })
    );
  }

  /**
   * PostProcessing Dates
   * Converting UTC to Local Date
   * Date must be in ISO 8601 format
   */
  private postProcessDates(obj: unknown): unknown {
    if (obj === null || obj === undefined) {
      return obj;
    }

    if (typeof obj !== 'object') {
      return obj;
    }

    for (const key of Object.keys(obj)) {
      const value = obj[key];
      const iso8601 = /^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(\.\d+)?(([+-]\d\d:\d\d)|Z)?$/;

      if (iso8601.test(value) === true) {
        obj[key] = new Date(value);
      } else if (typeof value === 'object') {
        this.postProcessDates(value);
      }
    }
  }
}

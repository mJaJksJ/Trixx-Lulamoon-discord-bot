import { BehaviorSubject, MonoTypeOperatorFunction, defer } from 'rxjs';
import { ReactiveUtils } from './reactive-utils';

export class TrixxLoadingSubject extends BehaviorSubject<boolean> {
  constructor() {
    super(false);
  }
  wrap<T>(): MonoTypeOperatorFunction<T> {
    return (x) =>
      defer(() => {
        this.next(true);
        return x;
      }).pipe(ReactiveUtils.clearLoading(this));
  }
}

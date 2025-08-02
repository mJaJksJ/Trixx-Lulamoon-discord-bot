import {
  OperatorFunction,
  Observer,
  finalize,
} from 'rxjs';
import { TrixxLoadingSubject } from './trixx-loading-subject';

export class ReactiveUtils {
  static clearLoading<T>(loading$: TrixxLoadingSubject | Observer<boolean>): OperatorFunction<T, T> {
    return finalize(() => loading$.next(false));
  }
}

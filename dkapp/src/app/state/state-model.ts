export enum LoadingStatus {
  IDLE = 'idle',
  LOADING = 'loading',
  SUCCESS = 'success',
  FAILED = 'failed',
}

export interface LoadingState<T> {
  data: T | null;
  status: LoadingStatus;
  error: string | null;
}

export function createInitialState<T>(): LoadingState<T> {
  return {
    data: null,
    status: LoadingStatus.IDLE,
    error: null,
  };
}
export function setLoading<T>(state: LoadingState<T>): LoadingState<T> {
  return {
    data: state.data,
    status: LoadingStatus.LOADING,
    error: null,
  };
}

export function setSuccess<T>(state: LoadingState<T>, data: T): LoadingState<T> {
  return {
    data,
    status: LoadingStatus.SUCCESS,
    error: null,
  };
}

export function setError<T>(state: LoadingState<T>, error: string): LoadingState<T> {
  return {
    data: state.data,
    status: LoadingStatus.FAILED,
    error,
  };
}

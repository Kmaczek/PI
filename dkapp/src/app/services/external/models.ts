export interface LoginResponse {
  token: string
}

export interface TokenModel {
  /** expiration time in UNIX timestamp */
  exp: number;
  email: string;
  unique_name: string;
  given_name: string;
  role: string[];
  token: string;
}
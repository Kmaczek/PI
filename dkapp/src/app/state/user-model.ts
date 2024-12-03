import { UserRoles } from "../models/user/user-roles";

export interface UserProfile {
  id: number;
  displayName: string;
  email :string;
  lastLoginUtc: Date;
  userRoles: Array<UserRoles>;
}
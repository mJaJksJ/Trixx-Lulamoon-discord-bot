import { NbAuthOAuth2JWTToken } from '@nebular/auth';
import { Permission } from '../../../api/models';

export interface TrixxAuthJWTTokenPayload {
  access_token: string;
  refresh_token: string;
  permissions: Permission[];
}
export class TrixxAuthJWTToken extends NbAuthOAuth2JWTToken {
  public readonly permissions: Permission[];

  constructor(token: any, ownerStrategyName: string, createdAt?: Date) {
    super(token, ownerStrategyName, createdAt);
    const payload = token as TrixxAuthJWTTokenPayload;
    this.permissions = payload.permissions;
  }
}

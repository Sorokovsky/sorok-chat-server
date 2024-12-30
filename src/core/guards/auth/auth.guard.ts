import {
  CanActivate,
  ExecutionContext,
  Injectable,
  UnauthorizedException,
} from "@nestjs/common";
import { AuthService } from "@features/auth/auth.service";
import { Request, Response } from "express";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private readonly authService: AuthService) {}

  public async canActivate(context: ExecutionContext): Promise<boolean> {
    const request: Request = context.switchToHttp().getRequest();
    const response: Response = context.switchToHttp().getResponse();
    const isAuthenticated: boolean = await this.authService.isAuthenticated(
      request,
      response,
    );
    if (isAuthenticated === false) {
      throw new UnauthorizedException();
    }
    return isAuthenticated;
  }
}

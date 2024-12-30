import { Injectable } from "@nestjs/common";
import {
  AUTHORIZATION_HEADER_NAME,
  BEARER_TOKEN_PREFIX,
} from "../../core/constants/default.constant";
import { Request, Response } from "express";

@Injectable()
export class BearerStorageService {
  public setToken(response: Response, token: string): void {
    response.setHeader(
      AUTHORIZATION_HEADER_NAME,
      `${BEARER_TOKEN_PREFIX}${token}`,
    );
  }

  public deleteToken(response: Response): void {
    this.setToken(response, "");
  }

  public getToken(request: Request): string | null {
    const authorization: string | null =
      request.header(AUTHORIZATION_HEADER_NAME) || null;
    if (authorization === null) {
      return null;
    }
    return authorization.replace(BEARER_TOKEN_PREFIX, "");
  }
}

import { Injectable } from "@nestjs/common";
import { getHttpContext } from "../../utils/http-context";
import {
  AUTHORIZATION_HEADER_NAME,
  BEARER_TOKEN_PREFIX,
} from "../../core/constants/default.constant";

@Injectable()
export class BearerStorageService {
  public setToken(token: string): void {
    const { response } = getHttpContext();
    response.setHeader(
      AUTHORIZATION_HEADER_NAME,
      `${BEARER_TOKEN_PREFIX}${token}`,
    );
  }
}

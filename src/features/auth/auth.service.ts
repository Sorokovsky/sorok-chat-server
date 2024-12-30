import { Injectable } from "@nestjs/common";
import { UsersService } from "../users/users.service";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { TokensService } from "../tokens/tokens.service";
import { TokensDto } from "../../core/contracts/dto/tokens.dto";
import { CookiesService } from "../cookies/cookies.service";
import {
  REFRESH_TOKEN_NAME,
  REQUEST_USER_KEY,
} from "../../core/constants/default.constant";
import { BearerStorageService } from "../bearer-storage/bearer-storage.service";
import { request, Request, Response } from "express";
import { TokenPayload } from "../../core/contracts/token-payload";

@Injectable()
export class AuthService {
  constructor(
    private readonly usersService: UsersService,
    private readonly tokensService: TokensService,
    private readonly cookiesService: CookiesService,
    private readonly bearerStorageService: BearerStorageService,
  ) {}

  public async register(
    response: Response,
    newUser: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const createdUser: GetUserDto = await this.usersService.create(
      newUser,
      avatar,
    );
    await this.authenticate(response, createdUser);
    return createdUser;
  }

  public async isAuthenticated(
    request: Request,
    response: Response,
  ): Promise<boolean> {
    const refreshToken: string | null = await this.cookiesService.getCookie(
      request,
      REFRESH_TOKEN_NAME,
    );
    const accessToken: string | null =
      this.bearerStorageService.getToken(request);
    const isAccessTokenValid: boolean = await this.processToken(
      response,
      accessToken,
    );
    if (isAccessTokenValid) {
      return true;
    }
    return await this.processToken(response, refreshToken);
  }

  private async authenticate(
    response: Response,
    user: GetUserDto,
  ): Promise<void> {
    const { accessToken, refreshToken }: TokensDto =
      await this.tokensService.generateTokens({ id: user.id });
    await this.cookiesService.setCookie(
      response,
      REFRESH_TOKEN_NAME,
      refreshToken,
    );
    this.bearerStorageService.setToken(response, accessToken);
    request[REQUEST_USER_KEY] = user;
  }

  private async verifyToken<T extends object>(
    token: string,
  ): Promise<T | null> {
    try {
      return await this.tokensService.verifyToken<T>(token);
    } catch {
      return null;
    }
  }

  public async logout(response: Response): Promise<void> {
    await this.cookiesService.deleteCookie(response, REFRESH_TOKEN_NAME);
    this.bearerStorageService.deleteToken(response);
  }

  private async processToken(
    response: Response,
    token: string,
  ): Promise<boolean> {
    const payload: TokenPayload | null = await this.verifyToken(token);
    if (payload === null) {
      return false;
    }
    const user: GetUserDto | null = await this.usersService.getById(payload.id);
    if (user === null) return false;
    await this.authenticate(response, user);
    return true;
  }
}

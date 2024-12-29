import { Injectable } from "@nestjs/common";
import { UsersService } from "../users/users.service";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { TokensService } from "../tokens/tokens.service";
import { TokensDto } from "../../core/contracts/dto/tokens.dto";
import { CookiesService } from "../cookies/cookies.service";
import { REFRESH_TOKEN_NAME } from "../../core/constants/default.constant";
import { getHttpContext } from "../../utils/http-context";
import { BearerStorageService } from "../bearer-storage/bearer-storage.service";

@Injectable()
export class AuthService {
  constructor(
    private readonly usersService: UsersService,
    private readonly tokensService: TokensService,
    private readonly cookiesService: CookiesService,
    private readonly bearerStorageService: BearerStorageService,
  ) {}

  public async registrate(
    newUser: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const createdUser: GetUserDto = await this.usersService.create(
      newUser,
      avatar,
    );
    const { accessToken, refreshToken }: TokensDto =
      await this.tokensService.generateTokens(createdUser);
    await this.cookiesService.setCookie(REFRESH_TOKEN_NAME, refreshToken);
    this.bearerStorageService.setToken(accessToken);
    return createdUser;
  }

  private setBearerToken(token: string): void {
    const { response } = getHttpContext();
    response.setHeader("Authorization", `Bearer ${token}`);
  }
}

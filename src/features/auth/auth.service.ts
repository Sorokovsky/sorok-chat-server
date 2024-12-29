import { Injectable } from "@nestjs/common";
import { UsersService } from "../users/users.service";
import { CreateUserDtoWithoutAvatar } from "../../core/contracts/dto/user/create-user.dto";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { TokensService } from "../tokens/tokens.service";
import { TokensDto } from "../../core/contracts/dto/tokens.dto";

@Injectable()
export class AuthService {
  constructor(
    private readonly usersService: UsersService,
    private readonly tokensService: TokensService,
  ) {}

  public async registrate(
    newUser: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const createdUser: GetUserDto = await this.usersService.create(
      newUser,
      avatar,
    );
    const tokens: TokensDto =
      await this.tokensService.generateTokens(createdUser);
    return createdUser;
  }
}

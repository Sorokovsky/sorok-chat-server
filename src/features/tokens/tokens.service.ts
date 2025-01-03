import { Injectable } from "@nestjs/common";
import { JwtService } from "@nestjs/jwt";
import { TokensDto } from "@contracts/dto/tokens.dto";
import { ConfigService } from "@nestjs/config";
import { EnvParameters } from "@contracts/env-parameters.enum";

@Injectable()
export class TokensService {
  constructor(
    private readonly jwtService: JwtService,
    private readonly configService: ConfigService,
  ) {}

  public async generateTokens<T extends object>(
    payload: T,
  ): Promise<TokensDto> {
    return {
      accessToken: await this.generateAccessToken(payload),
      refreshToken: await this.generateRefreshToken(payload),
    };
  }

  public async generateRefreshToken<T extends object>(
    payload: T,
  ): Promise<string> {
    return await this.generateToken(payload, "7d");
  }

  public async generateAccessToken<T extends object>(
    payload: T,
  ): Promise<string> {
    return await this.generateToken(payload, "1h");
  }

  public async verifyToken<T extends object>(token: string): Promise<T> {
    const secret: string = this.configService.get<string>(
      EnvParameters.JWT_SECRET,
    );
    return await this.jwtService.verifyAsync(token, { secret });
  }

  private async generateToken<T extends object>(
    payload: T,
    lifeTime: number | string,
  ): Promise<string> {
    const secret: string = this.configService.get<string>(
      EnvParameters.JWT_SECRET,
    );
    return await this.jwtService.signAsync(
      { ...payload },
      {
        expiresIn: lifeTime,
        secret,
      },
    );
  }
}

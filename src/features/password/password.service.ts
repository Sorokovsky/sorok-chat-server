import { Injectable } from "@nestjs/common";
import { hash, compare } from "bcrypt";
import { ConfigService } from "@nestjs/config";
import { EnvParameters } from "../../core/contracts/env-parameters.enum";

@Injectable()
export class PasswordService {
  constructor(private readonly configService: ConfigService) {}

  public async hash(password: string): Promise<string> {
    const salt: number = this.configService.get<number>(
      EnvParameters.PASSWORD_SALT,
    );
    return await hash(password, +salt);
  }

  public async isValidPassword(
    password: string,
    hash: string,
  ): Promise<boolean> {
    return await compare(password, hash);
  }
}

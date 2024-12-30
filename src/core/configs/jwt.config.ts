import { ConfigService } from "@nestjs/config";
import { JwtModuleOptions } from "@nestjs/jwt";
import { EnvParameters } from "@contracts/env-parameters.enum";

export const getJwtConfig = async (
  configService: ConfigService,
): Promise<JwtModuleOptions> => {
  const secret: string = configService.get(EnvParameters.JWT_SECRET);
  return {
    secret: secret,
    global: true,
    secretOrPrivateKey: secret,
    privateKey: secret,
    publicKey: secret,
  };
};

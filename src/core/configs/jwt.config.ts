import { ConfigService } from "@nestjs/config";
import { JwtModuleOptions } from "@nestjs/jwt";
import { EnvParameters } from "../contracts/env-parameters.enum";

export const getJwtConfig = async (
  configService: ConfigService,
): Promise<JwtModuleOptions> => ({
  secret: configService.get(EnvParameters.JWT_SECRET),
});

import { ConfigService } from "@nestjs/config";
import { TypeOrmModuleOptions } from "@nestjs/typeorm";
import { EnvParameters } from "../contracts/env-parameters.enum";

export const getTypeOrmConfig = async (
  configService: ConfigService,
): Promise<TypeOrmModuleOptions> => ({
  type: "postgres",
  database: configService.get<string>(EnvParameters.DB_NAME),
  host: configService.get<string>(EnvParameters.DB_HOST),
  password: configService.get<string>(EnvParameters.DB_PASSWORD),
  port: configService.get<number>(EnvParameters.DB_PORT),
  username: configService.get<string>(EnvParameters.DB_USER),
  synchronize: true,
  logging: true,
  autoLoadEntities: true,
  entities: ["*/**/*.entity.js"],
});

import { ConsoleLogger, INestApplication } from "@nestjs/common";
import { DEFAULT_PORT } from "../constants/default.constant";
import { ConfigService } from "@nestjs/config";
import { EnvParameters } from "../contracts/env-parameters.enum";

export const run = async (application: INestApplication): Promise<void> => {
  const configService: ConfigService = application.get(ConfigService);
  const logger: ConsoleLogger = application.get(ConsoleLogger);
  const envPort: number = await configService.get(EnvParameters.PORT);
  const port: number = envPort ?? DEFAULT_PORT;
  await application.listen(port);
  logger.log(`Server is running on port ${port}`, "Server");
};

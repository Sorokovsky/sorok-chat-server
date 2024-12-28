import { ConsoleLogger, INestApplication } from "@nestjs/common";
import { DEFAULT_PORT } from "../constant/default.constant";
import { ConfigService } from "@nestjs/config";

export const run = async (application: INestApplication): Promise<void> => {
  const configService: ConfigService = application.get(ConfigService);
  const logger: ConsoleLogger = application.get(ConsoleLogger);
  const port: number = (await configService.get("PORT")) ?? DEFAULT_PORT;
  await application.listen(port);
  logger.log(`Server is running on port ${port}`, "Server");
};

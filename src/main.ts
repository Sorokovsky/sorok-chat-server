import { NestFactory } from "@nestjs/core";
import { AppModule } from "./app.module";
import { ConsoleLogger, INestApplication } from "@nestjs/common";
import { ConfigService } from "@nestjs/config";
import { DEFAULT_PORT } from "./constant/default.constant";

async function bootstrap(): Promise<void> {
  const app: INestApplication = await NestFactory.create(AppModule);
  app.useLogger(new ConsoleLogger());
  const configService: ConfigService = app.get(ConfigService);
  const logger: ConsoleLogger = app.get(ConsoleLogger);
  const port: number = (await configService.get("PORT")) ?? DEFAULT_PORT;
  await app.listen(port);
  logger.log(`Server is running on port ${port}`, "Server");
}

bootstrap();

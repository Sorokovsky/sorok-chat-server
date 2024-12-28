import { NestFactory } from "@nestjs/core";
import { AppModule } from "./app.module";
import { INestApplication } from "@nestjs/common";
import { run } from "./configs/run.config";
import { prepareSwagger } from "./configs/swagger.config";
import { prepareApplication } from "./configs/application.config";

async function bootstrap(): Promise<void> {
  const application: INestApplication = await NestFactory.create(AppModule);
  prepareApplication(application);
  prepareSwagger(application);
  await run(application);
}

bootstrap();

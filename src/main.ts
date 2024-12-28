import { NestFactory } from "@nestjs/core";
import { AppModule } from "./app.module";
import { INestApplication } from "@nestjs/common";
import { run } from "./config/run.config";

async function bootstrap(): Promise<void> {
  const application: INestApplication = await NestFactory.create(AppModule);
  await run(application);
}

bootstrap();

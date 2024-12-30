import { INestApplication } from "@nestjs/common";
import { DocumentBuilder, SwaggerModule } from "@nestjs/swagger";
import {
  APPLICATION_NAME,
  SWAGGER_PREFIX,
  VERSION,
} from "@constants/default.constant";

export const prepareSwagger = (application: INestApplication): void => {
  const config = new DocumentBuilder()
    .setTitle(APPLICATION_NAME)
    .setVersion(VERSION)
    .addBearerAuth()
    .build();
  const documentFactory = () =>
    SwaggerModule.createDocument(application, config);
  SwaggerModule.setup(SWAGGER_PREFIX, application, documentFactory, {
    swaggerOptions: {
      defaultModelsExpandDepth: -1,
    },
  });
};

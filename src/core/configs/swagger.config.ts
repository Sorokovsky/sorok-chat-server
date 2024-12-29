import { INestApplication } from "@nestjs/common";
import { DocumentBuilder, SwaggerModule } from "@nestjs/swagger";
import { SWAGGER_PREFIX, VERSION } from "../constants/default.constant";

export const prepareSwagger = (application: INestApplication) => {
  const config = new DocumentBuilder()
    .setTitle("SorokChat Server")
    .setVersion(VERSION)
    .build();
  const documentFactory = () =>
    SwaggerModule.createDocument(application, config);
  SwaggerModule.setup(SWAGGER_PREFIX, application, documentFactory, {
    swaggerOptions: {
      defaultModelsExpandDepth: -1,
    },
  });
};

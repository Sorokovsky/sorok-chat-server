import { INestApplication, ValidationPipe } from "@nestjs/common";
import { API_PREFIX } from "../constants/default.constant";

export const prepareApplication = (application: INestApplication) => {
  application.setGlobalPrefix(API_PREFIX);
  application.useGlobalPipes(new ValidationPipe());
};

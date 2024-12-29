import { INestApplication, ValidationPipe } from "@nestjs/common";
import { API_PREFIX } from "../constants/default.constant";
import * as cookieParser from "cookie-parser";

export const prepareApplication = (application: INestApplication) => {
  application.setGlobalPrefix(API_PREFIX);
  application.enableCors();
  application.use(cookieParser());
  application.useGlobalPipes(new ValidationPipe());
};

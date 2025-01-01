import { join } from "node:path";

export const DEFAULT_PORT: number = 5000;
export const MEDIA_FOLDER_NAME: string = "images";
export const DEFAULT_AVATAR_PATH: string = join(
  MEDIA_FOLDER_NAME,
  "avatar.png",
);
export const DEFAULT_CHANNEL_IMAGE_PATH: string = join(
  MEDIA_FOLDER_NAME,
  "channel.png",
);
export const API_PREFIX: string = "api";
export const SWAGGER_PREFIX: string = "swagger";
export const VERSION: string = "1.0";
export const APPLICATION_NAME: string = "Sorok Chat";
export const SERVER_FOLDER: string = join(
  __dirname,
  "..",
  "..",
  "..",
  "uploads",
);
export const MIN_PASSWORD_LENGTH: number = 8;
export const MAX_PASSWORD_LENGTH: number = 20;
export const REFRESH_TOKEN_NAME: string = "refresh-token";
export const AUTHORIZATION_HEADER_NAME: string = "Authorization";
export const BEARER_TOKEN_PREFIX: string = "Bearer ";
export const REQUEST_USER_KEY: string = "user";

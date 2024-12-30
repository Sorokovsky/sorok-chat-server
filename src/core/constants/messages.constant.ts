import {
  MAX_PASSWORD_LENGTH,
  MIN_PASSWORD_LENGTH,
} from "@constants/default.constant";

export const MAX_PASSWORD_MESSAGE = `Password must less or equals than ${MAX_PASSWORD_LENGTH} characters long`;
export const MIN_PASSWORD_MESSAGE = `Password must more or equals than ${MIN_PASSWORD_LENGTH} characters long`;
export const getNotFoundMessage = (
  entity: string,
  key: string,
  value: object,
) => `${entity} by ${key} == ${value} not found.`;
export const getAlreadyExistsMessage = (
  entity: string,
  key: string,
  value: object,
) => `${entity} by ${key} == ${value} already exists.`;
export const INVALID_PASSWORD_MESSAGE = "Invalid password.";

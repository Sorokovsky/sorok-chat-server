import { BadRequestException } from "@nestjs/common";

export class AlreadyExistsException extends BadRequestException {
  constructor(entity: string, key: string, value: object) {
    super(`${entity} by ${key} == ${value} already exists.`);
  }
}

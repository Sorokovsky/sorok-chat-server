import { BadRequestException } from "@nestjs/common";

export class NotFoundException extends BadRequestException {
  constructor(entity: string, key: string, value: object) {
    super(`${entity} by ${key} == ${value} not found.`);
  }
}

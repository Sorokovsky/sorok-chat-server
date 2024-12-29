import { BadRequestException } from "@nestjs/common";
import { getAlreadyExistsMessage } from "../../constants/messages.constant";

export class AlreadyExistsException extends BadRequestException {
  constructor(entity: string, key: string, value: any) {
    super(getAlreadyExistsMessage(entity, key, value));
  }
}

import { NotFoundException as NotFound } from "@nestjs/common";
import { getNotFoundMessage } from "../../constants/messages.constant";

export class NotFoundException extends NotFound {
  constructor(entity: string, key: string, value: any) {
    super(getNotFoundMessage(entity, key, value));
  }
}

import { AlreadyExistsException } from "@exceptions/base/already-exists.exception";
import { MessageEntity } from "@entities/message.entity";

export class MessageAlreadyExistsException extends AlreadyExistsException {
  constructor(key: keyof MessageEntity, value: any) {
    super("Message", key, value);
  }
}

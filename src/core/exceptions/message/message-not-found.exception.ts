import { NotFoundException } from "../base/not-found.exception";
import { MessageEntity } from "../../entities/message.entity";

export class MessageNotFoundException extends NotFoundException {
  constructor(key: keyof MessageEntity, value: any) {
    super("Message", key, value);
  }
}

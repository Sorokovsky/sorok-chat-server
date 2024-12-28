import { NotFoundException } from "../base/not-found.exception";

export class MessageNotFoundException extends NotFoundException {
  constructor(key: string, value: object) {
    super("Message", key, value);
  }
}

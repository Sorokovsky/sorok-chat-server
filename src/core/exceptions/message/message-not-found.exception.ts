import { NotFoundException } from "../base/not-found.exception";

export class MessageNotFoundException extends NotFoundException {
  constructor(key: string, value: any) {
    super("Message", key, value);
  }
}

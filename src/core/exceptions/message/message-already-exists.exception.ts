import { AlreadyExistsException } from "../base/already-exists.exception";

export class MessageAlreadyExistsException extends AlreadyExistsException {
  constructor(key: string, value: any) {
    super("Message", key, value);
  }
}

import { AlreadyExistsException } from "../base/already-exists.exception";

export class MessageAlreadyExistsException extends AlreadyExistsException {
  constructor(key: string, value: object) {
    super("Message", key, value);
  }
}

import { AlreadyExistsException } from "../base/already-exists.exception";

export class UserAlreadyExists extends AlreadyExistsException {
  constructor(key: string, value: object) {
    super("User", key, value);
  }
}

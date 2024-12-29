import { AlreadyExistsException } from "../base/already-exists.exception";

export class UserAlreadyExists extends AlreadyExistsException {
  constructor(key: string, value: any) {
    super("User", key, value);
  }
}

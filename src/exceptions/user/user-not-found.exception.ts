import { NotFoundException } from "../base/not-found.exception";

export class UserNotFoundException extends NotFoundException {
  constructor(key: string, value: object) {
    super("User", key, value);
  }
}

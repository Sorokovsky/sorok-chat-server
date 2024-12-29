import { AlreadyExistsException } from "../base/already-exists.exception";
import { UserEntity } from "../../entities/user.entity";

export class UserAlreadyExists extends AlreadyExistsException {
  constructor(key: keyof UserEntity, value: any) {
    super("User", key, value);
  }
}

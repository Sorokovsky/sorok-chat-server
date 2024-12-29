import { NotFoundException } from "../base/not-found.exception";
import { UserEntity } from "../../entities/user.entity";

export class UserNotFoundException extends NotFoundException {
  constructor(key: keyof UserEntity, value: any) {
    super("User", key, value);
  }
}

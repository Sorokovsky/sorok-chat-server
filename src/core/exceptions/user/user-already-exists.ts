import { AlreadyExistsException } from "../base/already-exists.exception";
import { GetUserDto } from "../../contracts/dto/user/get-user.dto";

export class UserAlreadyExists extends AlreadyExistsException {
  constructor(key: keyof GetUserDto, value: any) {
    super("User", key, value);
  }
}

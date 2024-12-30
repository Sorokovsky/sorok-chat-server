import { NotFoundException } from "@exceptions/base/not-found.exception";
import { GetUserDto } from "@contracts/dto/user/get-user.dto";

export class UserNotFoundException extends NotFoundException {
  constructor(key: keyof GetUserDto, value: any) {
    super("User", key, value);
  }
}

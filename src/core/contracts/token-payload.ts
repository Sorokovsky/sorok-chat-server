import { GetUserDto } from "@contracts/dto/user/get-user.dto";

export type TokenPayload = Pick<GetUserDto, "id">;

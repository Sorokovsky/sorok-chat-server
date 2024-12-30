import { GetUserDto } from "./dto/user/get-user.dto";

export type TokenPayload = Pick<GetUserDto, "id">;

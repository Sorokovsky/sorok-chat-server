import { createParamDecorator, ExecutionContext } from "@nestjs/common";
import { Request } from "express";
import { GetUserDto } from "../contracts/dto/user/get-user.dto";
import { REQUEST_USER_KEY } from "../constants/default.constant";

export const CurrentUser = createParamDecorator(
  (data: keyof GetUserDto, context: ExecutionContext) => {
    const request: Request = context.switchToHttp().getRequest();
    const user: GetUserDto = request[REQUEST_USER_KEY];
    return data ? user[data] : user;
  },
);

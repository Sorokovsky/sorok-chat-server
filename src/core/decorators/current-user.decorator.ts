import { createParamDecorator, ExecutionContext } from "@nestjs/common";
import { Request } from "express";
import { GetUserDto } from "../contracts/dto/user/get-user.dto";

export const CurrentUser = () =>
  createParamDecorator(
    (data: keyof GetUserDto | undefined, context: ExecutionContext) => {
      const request: Request = context.switchToHttp().getRequest();
      const user: GetUserDto = request["user"];
      return data ? user[data] : user;
    },
  );

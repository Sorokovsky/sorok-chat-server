import { createParamDecorator, ExecutionContext } from "@nestjs/common";
import { UserEntity } from "../entities/user.entity";
import { Request } from "express";

export const CurrentUser = () =>
  createParamDecorator(
    (data: keyof UserEntity | undefined, context: ExecutionContext) => {
      const request: Request = context.switchToHttp().getRequest();
      const user: UserEntity = request["user"];
      return data ? user[data] : user;
    },
  );

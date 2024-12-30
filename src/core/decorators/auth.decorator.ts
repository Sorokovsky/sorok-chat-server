import { applyDecorators, UseGuards } from "@nestjs/common";
import { AuthGuard } from "../guards/auth/auth.guard";
import { ApiUnauthorizedResponse } from "@nestjs/swagger";
import { ErrorDto } from "../contracts/dto/error.dto";

export const Auth = () =>
  applyDecorators(
    UseGuards(AuthGuard),
    ApiUnauthorizedResponse({
      description: "Unauthorized.",
      type: ErrorDto,
    }),
  );

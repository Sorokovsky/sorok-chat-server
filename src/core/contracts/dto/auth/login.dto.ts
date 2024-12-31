import { IsEmail, IsString, MaxLength, MinLength } from "class-validator";
import {
  MAX_PASSWORD_LENGTH,
  MIN_PASSWORD_LENGTH,
} from "@constants/default.constant";
import {
  MAX_PASSWORD_MESSAGE,
  MIN_PASSWORD_MESSAGE,
} from "@constants/messages.constant";
import { ApiProperty } from "@nestjs/swagger";

export class LoginDto {
  @IsString()
  @IsEmail()
  @ApiProperty({ default: "Sorokovskys@ukr.net" })
  public email: string;

  @IsString()
  @MinLength(MIN_PASSWORD_LENGTH, {
    message: MIN_PASSWORD_MESSAGE,
  })
  @MaxLength(MAX_PASSWORD_LENGTH, {
    message: MAX_PASSWORD_MESSAGE,
  })
  @ApiProperty({ default: "<USER_PASSWORD>" })
  public password: string;
}

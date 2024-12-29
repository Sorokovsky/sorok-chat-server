import {
  IsEmail,
  IsOptional,
  IsString,
  MaxLength,
  MinLength,
} from "class-validator";
import {
  MAX_PASSWORD_LENGTH,
  MIN_PASSWORD_LENGTH,
} from "../../../constants/default.constant";
import {
  MAX_PASSWORD_MESSAGE,
  MIN_PASSWORD_MESSAGE,
} from "../../../constants/messages.constant";
import { ApiProperty, OmitType } from "@nestjs/swagger";

export class CreateUserDto {
  @ApiProperty({ default: "Sorokovskys@ukr.net" })
  @IsString()
  @IsEmail()
  public email: string;

  @ApiProperty({ default: "<USER_PASSWORD>" })
  @IsString()
  @MinLength(MIN_PASSWORD_LENGTH, {
    message: MIN_PASSWORD_MESSAGE,
  })
  @MaxLength(MAX_PASSWORD_LENGTH, {
    message: MAX_PASSWORD_MESSAGE,
  })
  public password: string;

  @ApiProperty({ default: "Andrey", required: false })
  @IsString()
  @IsOptional()
  public name?: string;

  @ApiProperty({ default: "Sorokovsky", required: false })
  @IsString()
  @IsOptional()
  public surname?: string;

  @ApiProperty({ default: "Ivanovich", required: false })
  @IsString()
  @IsOptional()
  public middleName?: string;

  @ApiProperty({ type: "string", format: "binary", required: false })
  @IsOptional()
  public avatar?: Express.Multer.File;
}

export class CreateUserDtoWithoutAvatar extends OmitType(CreateUserDto, [
  "avatar",
]) {}

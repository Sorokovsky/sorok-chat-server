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
} from "@constants/default.constant";
import {
  MAX_PASSWORD_MESSAGE,
  MIN_PASSWORD_MESSAGE,
} from "@constants/messages.constant";
import { ApiPropertyOptional, OmitType } from "@nestjs/swagger";

export class UpdateUserDto {
  @ApiPropertyOptional({ default: "" })
  @IsString()
  @IsOptional()
  @IsEmail()
  public email?: string;

  @ApiPropertyOptional({ default: "" })
  @IsString()
  @IsOptional()
  @MinLength(MIN_PASSWORD_LENGTH, { message: MIN_PASSWORD_MESSAGE })
  @MaxLength(MAX_PASSWORD_LENGTH, { message: MAX_PASSWORD_MESSAGE })
  public password?: string;

  @ApiPropertyOptional({ default: "" })
  @IsString()
  @IsOptional()
  public surname?: string;

  @ApiPropertyOptional({ default: "" })
  @IsString()
  @IsOptional()
  public name?: string;

  @ApiPropertyOptional({ default: "" })
  @IsString()
  @IsOptional()
  public middleName?: string;

  @ApiPropertyOptional({
    default: "",
    type: "string",
    format: "binary",
  })
  @IsOptional()
  public avatar?: Express.Multer.File;
}

export class UpdateUserDtoWithoutAvatar extends OmitType(UpdateUserDto, [
  "avatar",
]) {}

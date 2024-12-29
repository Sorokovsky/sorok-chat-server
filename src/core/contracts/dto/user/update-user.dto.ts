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

export class UpdateUserDto {
  @ApiProperty({ default: null, required: false })
  @IsString()
  @IsOptional()
  @IsEmail()
  public email?: string;

  @ApiProperty({ default: null, required: false })
  @IsString()
  @IsOptional()
  @MinLength(MIN_PASSWORD_LENGTH, { message: MIN_PASSWORD_MESSAGE })
  @MaxLength(MAX_PASSWORD_LENGTH, { message: MAX_PASSWORD_MESSAGE })
  public password?: string;

  @ApiProperty({ default: null, required: false })
  @IsString()
  @IsOptional()
  public surname?: string;

  @ApiProperty({ default: null, required: false })
  @IsString()
  @IsOptional()
  public name?: string;

  @ApiProperty({ default: null, required: false })
  @IsString()
  @IsOptional()
  public middleName?: string;

  @ApiProperty({
    default: null,
    required: false,
    type: "string",
    format: "binary",
  })
  @IsOptional()
  public avatar?: Express.Multer.File;
}

export class UpdateUserDtoWithoutAvatar extends OmitType(UpdateUserDto, [
  "avatar",
]) {}

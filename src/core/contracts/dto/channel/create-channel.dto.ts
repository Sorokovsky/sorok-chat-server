import { IsOptional, IsString } from "class-validator";
import { ApiProperty, OmitType } from "@nestjs/swagger";

export class CreateChannelDto {
  @IsString()
  @ApiProperty({ default: "Channel" })
  public name: string;

  @ApiProperty({ default: "Channel description", required: false })
  @IsString()
  @IsOptional()
  public description?: string;

  @ApiProperty({ type: "string", format: "binary", required: false })
  @IsOptional()
  public avatar: Express.Multer.File;
}

export class CreateChannelDtoWithoutAvatar extends OmitType(CreateChannelDto, [
  "avatar",
]) {}

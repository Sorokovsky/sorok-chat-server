import { IsOptional, IsString } from "class-validator";
import { ApiProperty, OmitType } from "@nestjs/swagger";

export class UpdateChannelDto {
  @IsString()
  @IsOptional()
  @ApiProperty({ default: "", required: false })
  public name: string;

  @ApiProperty({ default: "", required: false })
  @IsString()
  @IsOptional()
  public description?: string;

  @ApiProperty({ type: "string", format: "binary", required: false })
  @IsOptional()
  public avatar?: Express.Multer.File;

  @ApiProperty({ type: "string", format: "binary", required: false })
  @IsOptional()
  public image?: Express.Multer.File;
}

export class UpdateChannelDtoWithoutFiles extends OmitType(UpdateChannelDto, [
  "avatar",
  "image",
]) {}

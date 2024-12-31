import { BaseGetDto } from "@contracts/dto/base-get.dto";
import { ApiProperty } from "@nestjs/swagger";
import { GetUserDto } from "@contracts/dto/user/get-user.dto";

export class GetChannelDto extends BaseGetDto {
  @ApiProperty({ default: "Channel" })
  public name: string;

  @ApiProperty({ default: "Channel description" })
  public description: string;

  @ApiProperty({ default: "channels\\1\\images\\avatar.png" })
  public avatarPath: string;

  @ApiProperty({ default: "channels\\1\\images\\avatar.png" })
  public imagePath: string;

  @ApiProperty({ type: [GetUserDto] })
  members: GetUserDto[];
}

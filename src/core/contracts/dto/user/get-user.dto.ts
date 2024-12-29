import { ApiProperty } from "@nestjs/swagger";
import { ChannelEntity } from "../../../entities/channel.entity";
import { MessageEntity } from "../../../entities/message.entity";

export class GetUserDto {
  @ApiProperty({ default: 1 })
  public id: number;

  @ApiProperty({ default: Date.now() })
  public createdAt: Date;

  @ApiProperty({ default: Date.now() })
  public updatedAt: Date;

  @ApiProperty({ default: "Sorokovskys@ukr.net" })
  public email: string;

  @ApiProperty({ default: "Andrey" })
  public name: string;

  @ApiProperty({ default: "Sorokovsky" })
  public surname: string;

  @ApiProperty({ default: "Ivanovich" })
  public middleName: string;

  @ApiProperty({ default: "1\\images\\avatar.png" })
  public avatarPath: string;
}
